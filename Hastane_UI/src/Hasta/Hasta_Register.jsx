import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import './HastaStyle/Hasta_add.css';

function HastaRegister() {
    const navigate = useNavigate();

    const [name, setName] = useState("");
    const [surname, setSurname] = useState("");
    const [tc, setTc] = useState("");
    const [eposta, setEposta] = useState("");
    const [password, setPassword] = useState("");

    const [yukleniyor, setYukleniyor] = useState(false);
    const [hata, setHata] = useState(null);
    const [mesaj, setMesaj] = useState("");

    const handleSumbit = async (e) => {
        e.preventDefault();
        setYukleniyor(true);
        setHata(null);

        try {
            const cevap = await fetch('http://localhost:5160/api/Hasta/Register', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({
                    Name: name,
                    Surname: surname,
                    Tc: tc,
                    Eposta: eposta,
                    Password: password,
                }),
            });

            if (!cevap.ok) {
                throw new Error(`Sunucu hatası: HTTP ${cevap.status}`);
            }

            const veri = await cevap.json();
            setMesaj("Hasta başarıyla eklendi! Yönlendiriliyorsunuz...");
            setTimeout(() => navigate('/hasta-panel'), 1500);
            console.log("Server Response:", veri);

        } catch (err) {
            setHata(err.message);
        } finally {
            setYukleniyor(false);
        }
    };

    return (
        <div className="hasta-form-container">
            <h2>Hasta Kayıt Ol</h2>
            <form onSubmit={handleSumbit}>
                <div>
                    <label>Ad:</label>
                    <input type="text" value={name} onChange={(e) => setName(e.target.value)} required />
                </div>
                <div>
                    <label>Soyad:</label>
                    <input type="text" value={surname} onChange={(e) => setSurname(e.target.value)} required />
                </div>
                <div>
                    <label>TC:</label>
                    <input type="text" value={tc} onChange={(e) => setTc(e.target.value)} required />
                </div>
                <div>
                    <label>E-posta:</label>
                    <input type="email" value={eposta} onChange={(e) => setEposta(e.target.value)} required />
                </div>
                <div>
                    <label>Şifre:</label>
                    <input type="password" value={password} onChange={(e) => setPassword(e.target.value)} required />
                </div>

                <button type="submit" disabled={yukleniyor}>
                    {yukleniyor ? "Kaydediliyor..." : "Hasta Ekle"}
                </button>
            </form>

            {hata && <div className="error-message">Hata: {hata}</div>}
            {mesaj && <div className="success-message">{mesaj}</div>}
        </div>
    );
}

export default HastaRegister;
