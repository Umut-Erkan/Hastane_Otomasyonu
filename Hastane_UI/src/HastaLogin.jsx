import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import './Hasta_add.css'; // Reusing the same CSS to maintain consistency

function HastaLogin() {
    const navigate = useNavigate();
    const [tc, setTc] = useState("");
    const [password, setPassword] = useState("");

    const [yukleniyor, setYukleniyor] = useState(false);
    const [hata, setHata] = useState(null);
    const [mesaj, setMesaj] = useState("");

    const handleSubmit = async (e) => {
        e.preventDefault();
        setYukleniyor(true);
        setHata(null);
        setMesaj("");

        try {
            const cevap = await fetch('http://localhost:5160/api/Hasta/Login', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({
                    Tc: tc,
                    Password: password,
                }),
            });

            if (cevap.StatusCode != 200) {
                const errorData = await cevap.json().catch(() => ({}));
                throw new Error(errorData.mesaj || `Sunucu hatası: HTTP ${cevap.status}`);
            }

            const veri = await cevap.json();

            if (veri.accessToken) {
                localStorage.setItem('hastaToken', veri.accessToken);
            }

            if (localStorage.getItem('hastaToken') === null) {
                console.log("Hasta Token: null geldi");
            }

            setMesaj("Giriş başarılı! Yönlendiriliyorsunuz...");

            setTimeout(() => {
                navigate('/hasta-panel');
            }, 1000);

        } catch (err) {
            setHata(err.message);
        } finally {
            setYukleniyor(false);
        }
    };

    return (
        <div className="hasta-form-container">
            <h2>Hasta Girişi</h2>
            <form onSubmit={handleSubmit}>
                <div>
                    <label>TC Kimlik No:</label>
                    <input
                        type="text"
                        value={tc}
                        onChange={(e) => setTc(e.target.value)}
                        required
                    />
                </div>
                <div>
                    <label>Şifre:</label>
                    <input
                        type="password"
                        value={password}
                        onChange={(e) => setPassword(e.target.value)}
                        required
                    />
                </div>

                <button type="submit" disabled={yukleniyor}>
                    {yukleniyor ? "Giriş Yapılıyor..." : "Giriş Yap"}
                </button>
            </form>

            {hata && <div className="error-message">Hata: {hata}</div>}
            {mesaj && <div className="success-message" style={{ color: 'green', marginTop: '10px' }}>{mesaj}</div>}
        </div>
    );
}

export default HastaLogin;
