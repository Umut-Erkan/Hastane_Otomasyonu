import { useState } from 'react';

function HastaEkle() {

    const [name, setName] = useState("");
    const [surname, setSurname] = useState("");
    const [tc, setTc] = useState("");
    const [eposta, setEposta] = useState("");
    const [password, setPassword] = useState("");

    const [yukleniyor, setYukleniyor] = useState(false);
    const [hata, setHata] = useState(null);
    const [mesaj, setMesaj] = useState("");

    // Kaydetme fonksiyonu (Form gönderildiğinde çalışır)
    const handleSumbit = async (e) => {
        e.preventDefault(); // Sayfanın yenilenmesini engeller
        setYukleniyor(true);
        setHata(null);

        try {
            const cevap = await fetch('http://localhost:5160/api/Hasta/Login', {
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
            setMesaj("Hasta başarıyla eklendi!");
            console.log("Server Response:", veri);

        } catch (err) {
            setHata(err.message);
        } finally {
            setYukleniyor(false);
        }
    };

    return (
        <div style={{ padding: '20px', maxWidth: '400px' }}>
            <h2>Hasta Kayıt</h2>
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

            {hata && <div style={{ color: 'red' }}>Hata: {hata}</div>}
            {mesaj && <div style={{ color: 'green' }}>{mesaj}</div>}
        </div>
    );
}

export default HastaEkle;
