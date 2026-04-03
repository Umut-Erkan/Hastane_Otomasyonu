import { useState } from 'react';

// 1. Yeni bir Access Token almak için (Refresh Token ile)
export function RegenerateAccessTokenForm() {
    const [refreshToken, setRefreshToken] = useState("");
    const [mesaj, setMesaj] = useState(null);

    const handleGetAccessToken = async (e) => {
        e.preventDefault();
        try {
            // [FromBody] string beklendiği için JSON.stringify ile sadece string gönderiyoruz veya raw JSON formatında.
            // C# tarafında sadece string beklediği için refreshToken'ı çift tırnak içinde gönderiyoruz.
            const cevap = await fetch('http://localhost:5160/api/Token/AccessTokenRefresh', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(refreshToken), 
            });

            const veri = await cevap.json();

            if (cevap.ok) {
                setMesaj(`Başarılı! Yeni Access Token: ${veri.accessToken} (Süre: ${veri.expiresIn})`);
                // İsterseniz burada localStorage.setItem('hastaToken', veri.accessToken) yapabilirsiniz.
            } else {
                setMesaj(`Hata: ${veri.mesaj || 'İşlem başarısız'}`);
            }
        } catch (error) {
            setMesaj(`Bağlantı Hatası: ${error.message}`);
        }
    };

    return (
        <div style={{ padding: '20px', border: '1px solid #ccc', borderRadius: '8px', marginBottom: '20px', maxWidth: '400px' }}>
            <h3>Yeni Access Token Al</h3>
            <form onSubmit={handleGetAccessToken} style={{ display: 'flex', flexDirection: 'column', gap: '10px' }}>
                <label>Mevcut Refresh Token:</label>
                <input 
                    type="text" 
                    value={refreshToken} 
                    onChange={(e) => setRefreshToken(e.target.value)} 
                    placeholder="Refresh Token'ı buraya girin"
                    required 
                />
                <button type="submit">Access Token Getir</button>
            </form>
            {mesaj && <div style={{ marginTop: '10px', fontSize: '14px', wordBreak: 'break-all' }}>{mesaj}</div>}
        </div>
    );
}

// 2. Yeni bir Refresh Token almak için (E-posta, Şifre ve Rol ile)
export function RegenerateRefreshTokenForm() {
    const [eposta, setEposta] = useState("");
    const [password, setPassword] = useState("");
    const [role, setRole] = useState("Hasta"); // Default Hasta
    const [mesaj, setMesaj] = useState(null);

    const handleGetRefreshToken = async (e) => {
        e.preventDefault();
        try {
            const cevap = await fetch('http://localhost:5160/api/Token/RefreshToken', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({
                    Eposta: eposta,
                    Password: password,
                    Role: role
                }),
            });

            const veri = await cevap.json();

            if (cevap.ok) {
                setMesaj(`Başarılı! Yeni Refresh Token: ${veri.refreshToken} (Bitiş: ${new Date(veri.refreshTokenEndDate).toLocaleString()})`);
                // İsterseniz burada localStorage.setItem('refreshToken', veri.refreshToken) yapabilirsiniz.
            } else {
                setMesaj(`Hata: ${veri.mesaj || 'İşlem başarısız'}`);
            }
        } catch (error) {
            setMesaj(`Bağlantı Hatası: ${error.message}`);
        }
    };

    return (
        <div style={{ padding: '20px', border: '1px solid #ccc', borderRadius: '8px', maxWidth: '400px' }}>
            <h3>Yeni Refresh Token Al</h3>
            <form onSubmit={handleGetRefreshToken} style={{ display: 'flex', flexDirection: 'column', gap: '10px' }}>
                <label>E-posta:</label>
                <input type="email" value={eposta} onChange={(e) => setEposta(e.target.value)} required />
                
                <label>Şifre:</label>
                <input type="password" value={password} onChange={(e) => setPassword(e.target.value)} required />
                
                <label>Rol:</label>
                <select value={role} onChange={(e) => setRole(e.target.value)}>
                    <option value="Hasta">Hasta</option>
                    <option value="Doktor">Doktor</option>
                </select>

                <button type="submit">Refresh Token Getir</button>
            </form>
            {mesaj && <div style={{ marginTop: '10px', fontSize: '14px', wordBreak: 'break-all' }}>{mesaj}</div>}
        </div>
    );
}

// Kolayca sayfada gösterebilmek için ikisini saran ortak bir bileşen
export default function TokenManager() {
    return (
        <div style={{ display: 'flex', flexDirection: 'column', alignItems: 'center', margin: '40px' }}>
            <h2>Token Yönetim Ekranı</h2>
            <RegenerateAccessTokenForm />
            <RegenerateRefreshTokenForm />
        </div>
    );
}
