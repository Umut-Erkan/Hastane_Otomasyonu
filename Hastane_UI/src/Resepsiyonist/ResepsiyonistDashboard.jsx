import { useState, useEffect } from 'react';
import '../Hasta/HastaStyle/Hasta_add.css';
import { useLocation } from 'react-router-dom';

// JWT'yi decode etmek için yardımcı fonksiyon
function parseJwt(token) {
    try {
        var base64Url = token.split('.')[1];
        var base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
        var jsonPayload = decodeURIComponent(window.atob(base64).split('').map(function (c) {
            return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
        }).join(''));

        return JSON.parse(jsonPayload);
    } catch (e) {
        return null;
    }
}

function ResepsiyonistDashboard() {
    const [doktorlar, setDoktorlar] = useState([]);
    const [hata, setHata] = useState(null);
    const [yukleniyor, setYukleniyor] = useState(false);
    const [resepsiyonistAlan, setResepsiyonistAlan] = useState("");
    const location = useLocation();

    useEffect(() => {
        const fetchDoktorlar = async () => {
            const token = localStorage.getItem('resepsiyonistToken');
            if (!token) {
                setHata("Giriş yapılmamış. Lütfen giriş yapınız.");
                return;
            }


            const alan = location.state?.resepsiyonistData?.alan;

            setResepsiyonistAlan(alan);
            setYukleniyor(true);

            if (!alan) {
                setHata("Uzmanlık alanı bilgisi bulunamadı!");
                setYukleniyor(false);
                return;
            }

            try {
                const response = await fetch('http://localhost:5160/api/Doktor/DisplayDoktor', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                        'Authorization': `Bearer ${token}`
                    },
                    body: JSON.stringify({ Alan: alan })
                });

                if (!response.ok) {
                    const err = await response.json();
                    throw new Error(err.mesaj || "Bu alanda doktor bulunamadı veya sunucu hatası");
                }

                const data = await response.json();
                console.log("Doktorlar (API'den dönen ham veri): ", data);

                setDoktorlar(data)


            } catch (err) {
                console.error("Doktor listesi alınamadı:", err);
                setHata(err.message);
            } finally {
                setYukleniyor(false);
            }
        };

        fetchDoktorlar();
    }, [location.state]);

    return (
        <div className="hasta-form-container">
            <h2>{resepsiyonistAlan ? `İlgili Alan: ${resepsiyonistAlan}` : "Resepsiyonist Paneli"}</h2>
            <p>Eşleşen Doktor Listesi:</p>

            {hata && <div className="error-message">Hata: {hata}</div>}

            {yukleniyor ? (
                <p>Doktorlar yükleniyor...</p>
            ) : (
                <div style={{ marginTop: '20px' }}>
                    {doktorlar && doktorlar.length > 0 ? (
                        <ul className="doktor-listesi-view" style={{
                            display: 'block',
                            padding: 0,
                            margin: '20px 0',
                            position: 'static',
                            maxHeight: 'none',
                            overflow: 'visible'
                        }}>
                            {doktorlar.map((doktor, index) => (
                                <li
                                    key={doktor.Id || doktor.id || index}
                                    style={{
                                        padding: '12px 20px',
                                        border: '1px solid #e2e8f0',
                                        listStyleType: 'none',
                                        backgroundColor: '#ffffff',
                                        marginBottom: '12px',
                                        borderRadius: '10px',
                                        fontSize: '1rem',
                                        boxShadow: '0 4px 6px -1px rgba(0, 0, 0, 0.1), 0 2px 4px -1px rgba(0, 0, 0, 0.06)',
                                        display: 'flex',
                                        justifyContent: 'space-between',
                                        alignItems: 'center',
                                        transition: 'transform 0.2s ease'
                                    }}
                                >
                                    <span style={{ fontWeight: '600', color: '#1a202c' }}>
                                        {(doktor.Name || doktor.name || doktor.İsim || "İsim Belirsiz")} {(doktor.Surname || doktor.surname || doktor.Soyisim || "Soyisim Belirsiz")}
                                    </span>
                                    <span style={{ fontSize: '0.85rem', color: '#718096', fontStyle: 'italic' }}>
                                        {doktor.Eposta || doktor.eposta || ""}
                                    </span>
                                </li>
                            ))}
                        </ul>
                    ) : (
                        !hata && <p>Bu alanda doktor bulunamadı.</p>
                    )}
                </div>
            )}
        </div>
    );
}

export default ResepsiyonistDashboard;
