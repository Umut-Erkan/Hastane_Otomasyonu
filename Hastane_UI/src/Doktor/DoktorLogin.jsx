import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import '../Hasta/HastaStyle/Hasta_add.css';

function DoktorLogin() {
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
            const response = await fetch('http://localhost:5160/api/Doktor/Login', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({
                    Tc: tc,
                    Password: password,
                }),
            });
            const data = await response.json();

            if (response.status !== 200) {
                throw new Error(data.mesaj || `Sunucu hatası: HTTP ${response.status}`);
            }

            if (data.accessToken != null) {
                localStorage.setItem('doktorToken', data.accessToken);
            }

            setMesaj("Giriş başarılı! Yönlendiriliyorsunuz...");

            setTimeout(() => {
                navigate('/doktor-panel/randevu-goster');
            }, 500);

        } catch (err) {
            setHata(err.message);
        } finally {
            setYukleniyor(false);
        }
    };

    return (
        <div className="hasta-form-container">
            <h2>Doktor Girişi</h2>
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

            {hata && <div className="error-message">{hata}</div>}
            {mesaj && <div className="success-message" style={{ color: 'green', marginTop: '10px' }}>{mesaj}</div>}
        </div>
    );
}

export default DoktorLogin;
