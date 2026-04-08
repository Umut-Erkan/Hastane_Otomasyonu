import { useState, useEffect } from 'react';
import '../Hasta/HastaStyle/Hasta_add.css';

// JWT'yi decode etmek için yardımcı fonksiyon
function parseJwt(token) {
    try {
        var base64Url = token.split('.')[1];
        var base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
        var jsonPayload = decodeURIComponent(window.atob(base64).split('').map(function(c) {
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

    useEffect(() => {
        const fetchDoktorlar = async () => {
            const token = localStorage.getItem('resepsiyonistToken');
            if (!token) {
                setHata("Giriş yapılmamış. Lütfen giriş yapınız.");
                return;
            }

            const payload = parseJwt(token);
            if (!payload) {
                setHata("Geçersiz veya bozuk token.");
                return;
            }

            // Token içerisinden Alan bilgisini çekiyoruz (Claim isimlerine göre fallback eklendi)
            // Eğer Alan JWT payload'ına 'Alan' adlı bir özellik olarak eklenmişse direkt olarak alınır.
            const alan = payload.Alan || payload.alan || "Bilinmeyen Alan";

            setResepsiyonistAlan(alan);
            setYukleniyor(true);

            if (alan === "Bilinmeyen Alan") {
                setHata("Token içerisinde 'Alan' bilgisi bulunamadı!");
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
                setDoktorlar(data);

            } catch (err) {
                console.error("Doktor listesi alınamadı:", err);
                setHata(err.message);
            } finally {
                setYukleniyor(false);
            }
        };

        fetchDoktorlar();
    }, []);

    return (
        <div className="hasta-form-container">
            <h2>{resepsiyonistAlan ? `İlgili Alan: ${resepsiyonistAlan}` : "Resepsiyonist Paneli"}</h2>
            <p>Eşleşen Doktor Listesi:</p>
            
            {hata && <div className="error-message">Hata: {hata}</div>}
            
            {yukleniyor ? (
                <p>Doktorlar yükleniyor...</p>
            ) : (
                <div style={{ marginTop: '20px' }}>
                    {doktorlar.length > 0 ? (
                        <ul className="doktor-listesi" style={{ display: 'block', padding: 0 }}>
                            {doktorlar.map((doktor) => (
                                <li 
                                    key={doktor.id} 
                                    style={{ 
                                        padding: '10px 15px', 
                                        border: '1px solid #ddd', 
                                        listStyleType: 'none',
                                        backgroundColor: '#f9f9f9',
                                        marginBottom: '10px',
                                        borderRadius: '8px',
                                        fontSize: '1.1rem',
                                        boxShadow: '0 2px 5px rgba(0,0,0,0.05)'
                                    }}
                                >
                                    🧑‍⚕️ <strong>{doktor.name} {doktor.surname}</strong>
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
