import { useState, useEffect } from 'react';
import './HastaStyle/Hasta_add.css';

function HastaRandevuAl() {

    const [sikayet, setSikayet] = useState("");
    const [secilenDoktor, setSecilenDoktor] = useState(null);
    const [tarih, setTarih] = useState("");
    const [saat, setSaat] = useState("");

    const [alanlar, setAlanlar] = useState([]);
    const [secilenAlan, setSecilenAlan] = useState("");
    const [doktorlar, setDoktorlar] = useState([]);
    const [doktorListesiAcik, setDoktorListesiAcik] = useState(false);
    const [yukleniyor, setYukleniyor] = useState(false);
    const [hata, setHata] = useState(null);
    const [mesaj, setMesaj] = useState("");

    // Uzmanlık alanlarını component yüklendiğinde bir kez çek
    useEffect(() => {
        fetch('http://localhost:5160/api/Doktor/GetAlanlar', {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${localStorage.getItem('hastaToken')}`
            },
        })
            .then(response => response.json())
            .then(data => {
                setAlanlar(data);
                console.log("Gelen alan sayısı:", data.length);
            })
            .catch(err => console.error("Alan listesi alınamadı:", err));
    }, []);

    const handleAlanSecimi = (e) => {
        const alan = e.target.value;
        setSecilenAlan(alan);
        setSecilenDoktor(null); // sıfırla
        setDoktorlar([]); // temizle
        setHata(null);

        if (alan !== "") {
            const response = fetch('http://localhost:5160/api/Doktor/DisplayDoktor', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${localStorage.getItem('hastaToken')}`
                },
                body: JSON.stringify({ Alan: alan })
            })

                .then(response => {
                    if (!response.ok) {
                        return response.json().then(err => { throw new Error(err.mesaj || "Bu alanda doktor bulunamadı"); });
                    }
                    return response.json();


                })
                .then(data => {
                    setDoktorlar(data);
                })
                .catch(err => {
                    console.error("Doktor listesi alınamadı:", err);
                    setHata(err.message);
                });
        }
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        setYukleniyor(true);
        setHata(null);

        try {
            const response = await fetch('http://localhost:5160/api/Randevu/Randevu Al', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${localStorage.getItem('hastaToken')}`
                },
                body: JSON.stringify({
                    Şikayet: sikayet,
                    DoktorName: secilenDoktor ? secilenDoktor.name : "",
                    DoktorSurname: secilenDoktor ? secilenDoktor.surname : "",
                    Tarih: tarih,
                    Saat: saat ? `${saat}:00.0000000` : "",
                }),
            });



            const veri = await response.json();
            console.log(veri);

            if (response.status == 200) {
                setMesaj("Randevu başarıyla alındı!");
            }
            else {
                throw new Error(`${veri.mesaj || 'Sunucu hatası'}`);
            }
            console.log("Server Response:", response);

        } catch (err) {
            setHata(err.message);
        } finally {
            setYukleniyor(false);
        }
    };

    return (
        <div className="hasta-form-container">
            <h2>Randevu Al</h2>
            <form onSubmit={handleSubmit}>

                <div>
                    <label>Şikayetiniz:</label>
                    <textarea
                        value={sikayet}
                        onChange={(e) => setSikayet(e.target.value)}
                        placeholder="Şikayetinizi buraya yazın..."
                        required
                    />
                </div>

                <div className="doktor-secim-container">
                    <label>Alan Seçin:</label>
                    <select
                        value={secilenAlan}
                        onChange={handleAlanSecimi}
                        className="doktor-secim-kutu"
                        style={{ fontFamily: 'inherit', fontSize: '1rem', cursor: 'pointer', appearance: 'auto' }}
                    >
                        <option value="">Uzmanlık Alanı Seçin</option>
                        {alanlar.map((alan, index) => (
                            <option key={index} value={alan}>{alan}</option>
                        ))}
                    </select>
                </div>

                {secilenAlan && (
                    <div className="doktor-secim-container" style={{ marginTop: '15px' }}>
                        <label>Doktor Seçin:</label>
                        <div
                            className="doktor-secim-kutu"
                            onClick={() => setDoktorListesiAcik(!doktorListesiAcik)}
                        >
                            {secilenDoktor ? `${secilenDoktor.name} ${secilenDoktor.surname}` : "Doktor Seçmek İçin Tıklayın"}
                        </div>
                        {doktorListesiAcik && (
                            <ul className="doktor-listesi">
                                {doktorlar.map((doktor) => (
                                    <li
                                        key={doktor.id}
                                        onClick={() => {
                                            setSecilenDoktor(doktor);
                                            setDoktorListesiAcik(false);
                                        }}
                                    >
                                        {doktor.name} {doktor.surname}
                                    </li>
                                ))}
                            </ul>
                        )}
                    </div>
                )}

                <div>
                    <label>Randevu Tarihi:</label>
                    <input type="date" value={tarih} onChange={(e) => setTarih(e.target.value)} required />
                </div>
                <div>
                    <label>Randevu Saati:</label>
                    <select
                        value={saat}
                        onChange={(e) => setSaat(e.target.value)}
                        required
                        className="doktor-secim-kutu"
                        style={{ fontFamily: 'inherit', fontSize: '1rem', cursor: 'pointer', appearance: 'auto', marginTop: '5px', width: '100%' }}
                    >
                        <option value="">Saat Seçin</option>
                        {Array.from({ length: 9 }, (_, i) => i + 9).map(hour => {
                            const formattedHour = hour.toString().padStart(2, '0');
                            return (
                                <option key={hour} value={`${formattedHour}:00`}>
                                    {hour}.00
                                </option>
                            );
                        })}
                    </select>
                </div>

                <button type="submit" disabled={yukleniyor || !secilenDoktor}>
                    {yukleniyor ? "İşleniyor..." : "Randevu Al"}
                </button>
            </form>


            {hata && <div className="error-message">Hata: {hata}</div>}
            {mesaj && <div className="success-message">{mesaj}</div>}
        </div>
    );
}

export default HastaRandevuAl;
