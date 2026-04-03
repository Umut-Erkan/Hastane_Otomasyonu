import { useState } from 'react';
import './HastaStyle/Hasta_add.css';

function HastaRandevuAl() {

    const [tc, setTc] = useState("");
    const [sikayet, setSikayet] = useState(""); // Kullanıcı şikayeti
    const [secilenDoktor, setSecilenDoktor] = useState(null);
    const [tarih, setTarih] = useState("");
    const [saat, setSaat] = useState("");

    const [doktorListesiAcik, setDoktorListesiAcik] = useState(false);
    const [yukleniyor, setYukleniyor] = useState(false);
    const [hata, setHata] = useState(null);
    const [mesaj, setMesaj] = useState("");


    const doktorlar = fetch('http://localhost:5160/api/Doktor/GetDoktorSql', {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${localStorage.getItem('token')}`
        },
    }).then(response => response.json());

    console.log("gelen doktor sayısı: " + doktorlar.length);

    const handleSubmit = async (e) => {
        e.preventDefault();
        setYukleniyor(true);
        setHata(null);
    }


    try {
        // GÖNDERİLEN İSTEK
        const cevap = fetch('http://localhost:5160/api/Randevu/Randevu Al', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${localStorage.getItem('token')}`
            },
            body: JSON.stringify({
                Tc: parseInt(tc),
                Şikayet: sikayet,
                DoktorName: secilenDoktor ? secilenDoktor.isim : "",
                DoktorSurname: secilenDoktor ? secilenDoktor.soyisim : "",
                Tarih: tarih,
                Saat: saat,
            }),
        });

        if (!cevap.ok) {
            throw new Error(`Sunucu hatası: HTTP ${cevap.status}`);
        }

        const veri = cevap.json();
        setMesaj("Randevu başarıyla alındı!");
        console.log("Server Response:", veri);

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
                <label>Doktor Seçin:</label>
                <div
                    className="doktor-secim-kutu"
                    onClick={() => setDoktorListesiAcik(!doktorListesiAcik)}
                >
                    {secilenDoktor ? `${secilenDoktor.isim} ${secilenDoktor.soyisim}` : "Doktor Seçmek İçin Tıklayın"}
                </div>
                {doktorListesiAcik && (
                    <ul className="doktor-listesi">
                        {doktorlar.map((doktor) => (
                            <li
                                key={doktor.ıd}
                                onClick={() => {
                                    setSecilenDoktor(doktor);
                                    setDoktorListesiAcik(false);
                                }}
                            >
                                {doktor.name} {doktor.surname} {doktor.alan}
                            </li>
                        ))}
                    </ul>
                )}
            </div>
            <div>
                <label>Randevu Tarihi:</label>
                <input type="date" value={tarih} onChange={(e) => setTarih(e.target.value)} required />
            </div>
            <div>
                <label>Randevu Saati:</label>
                <input type="time" value={saat} onChange={(e) => setSaat(e.target.value)} required />
            </div>

            <button type="submit" disabled={yukleniyor || !secilenDoktor}>
                {yukleniyor ? "İşleniyor..." : "Randevu Al"}
            </button>
        </form>

        {hata && <div className="error-message">Hata: {hata}</div>}
        {mesaj && <div className="success-message">{mesaj}</div>}
    </div>
);

export default HastaRandevuAl;
