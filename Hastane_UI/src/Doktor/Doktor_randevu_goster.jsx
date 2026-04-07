import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import '../Hasta/HastaStyle/Hasta_add.css';

function DoktorRandevuGoster() {
    const navigate = useNavigate();

    const [randevular, setRandevular] = useState([]);
    const [yukleniyor, setYukleniyor] = useState(true);
    const [hata, setHata] = useState(null);

    useEffect(() => {
        const fetchRandevular = async () => {
            try {
                const token = localStorage.getItem('doktorToken');

                if (token === null) {
                    throw new Error("Token bulunamadı. Lütfen önce giriş yapın.");
                }

                const response = await fetch(`http://localhost:5160/api/Doktor/RandevuGoster`, {
                    method: 'GET',
                    headers: {
                        'Content-Type': 'application/json',
                        "Authorization": `Bearer ${token}`,
                    }
                });

                if (response.status === 401) {
                    throw new Error(`Giriş yapmanız gerekiyor: HTTP ${response.status}`);
                }

                if (response.status === 403) {
                    throw new Error(`Yetkisiz erişim: HTTP ${response.status}`);
                }

                if (response.status === 404) {
                    setRandevular([]);
                    setYukleniyor(false);
                    return;
                }

                if (!response.ok) {
                    throw new Error(`Bir hata oluştu: HTTP ${response.status}`);
                }

                const veri = await response.json();
                console.log("Server Response:", veri);

                const isArray = Array.isArray(veri);
                if (isArray) {
                    setRandevular(veri);
                } else if (veri && !veri.mesaj && !veri.message) {
                    setRandevular([veri]);
                } else {
                    setRandevular([]);
                }

            } catch (err) {
                setHata(err.message);
            } finally {
                setYukleniyor(false);
            }
        };

        fetchRandevular();
    }, []);

    return (
        <div className="hasta-form-container" style={{ maxWidth: '600px' }}>
            <h2>Gelen Randevularım</h2>

            {yukleniyor && <div>Yükleniyor...</div>}

            {hata && <div className="error-message">Hata: {hata}</div>}

            {!yukleniyor && randevular.length > 0 && (
                <div style={{ marginTop: '20px' }}>
                    <ul style={{ listStyleType: 'none', padding: 0 }}>
                        {randevular.map((randevu, index) => (
                            <li key={index} style={{ padding: '10px', border: '1px solid #ccc', margin: '5px 0', borderRadius: '5px', display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
                                <div>
                                    <strong>Hasta Adı:</strong> {randevu.hastaName} {randevu.hastaSurname} <br />
                                    <strong>Şikayet:</strong> {randevu.hastaŞikayet || "Belirtilmemiş"} <br />
                                    <strong>Tarih:</strong> {randevu.tarih || randevu.Tarih || "Belirtilmemiş"} <br />
                                    <strong>Saat:</strong> {randevu.saat || randevu.Saat || "Belirtilmemiş"}
                                </div>
                                <button 
                                    onClick={() => navigate('/doktor-panel/tedavi-yaz', { state: { randevuId: randevu.id || randevu.Id, hastaId: randevu.hastaId || randevu.HastaId } })} 
                                    style={{ padding: '5px 15px', cursor: 'pointer', borderRadius: '4px', border: 'none', backgroundColor: '#007bff', color: 'white', fontWeight: 'bold' }}
                                    title="Tedavi Yaz"
                                >
                                    Tedavi Yaz
                                </button>
                            </li>
                        ))}
                    </ul>
                </div>
            )}

            {!yukleniyor && !hata && randevular.length === 0 && (
                <div style={{ marginTop: '20px', color: '#666', fontWeight: 'bold' }}>Gelen randevunuz bulunmamaktadır.</div>
            )}
        </div>
    );
}

export default DoktorRandevuGoster;
