import { useState, useEffect } from 'react';
import { useNavigate, useLocation } from 'react-router-dom';
import '../Hasta/HastaStyle/Hasta_add.css';

function DoktorRandevuGoster() {
    const navigate = useNavigate();

    const [randevular, setRandevular] = useState([]);
    const [yukleniyor, setYukleniyor] = useState(true);
    const [hata, setHata] = useState(null);

    const location = useLocation();

    const getDoktorUserId = () => {
        // 1. location.state'ten
        if (location.state?.userId) return location.state.userId;
        // 2. localStorage'dan
        const stored = localStorage.getItem('doktorUserId');
        if (stored) return stored;
        // 3. JWT token'dan çözümle
        try {
            const token = localStorage.getItem('doktorToken');
            if (token) {
                const payload = JSON.parse(atob(token.split('.')[1]));
                const id = payload["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"];
                if (id) return id;
            }
        } catch (e) { console.error("Token çözümlenemedi:", e); }
        return null;
    };

    const userId = getDoktorUserId();

    useEffect(() => {
        const fetchRandevular = async () => {
            try {
                const token = localStorage.getItem('doktorToken');
                console.log(`Token ile giriş yapıldı, userId: ${userId}`);

                if (token === null) {
                    throw new Error("Token bulunamadı. Lütfen önce giriş yapın.");
                }

                if (!userId) {
                    throw new Error("Doktor bilgisi bulunamadı.");
                }

                const response = await fetch(`http://localhost:5160/api/Doktor/RandevuGoster?userId=${userId}`,
                    {
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
                                    onClick={() => navigate('/doktor-panel/tedavi-yaz', { state: { randevuId: randevu.id || randevu.Id, hastaId: randevu.hastaId || randevu.HastaId, userId: userId } })}
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
