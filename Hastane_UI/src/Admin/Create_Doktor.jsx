import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';

const BRANSLAR = [
    "Dahiliye", "Acil Tıp", "Kardiyoloji", "Nöroloji",
    "Ortopedi", "Pediatri", "Göz Hastalıkları", "KBB",
    "Psikiyatri", "Genel Cerrahi", "Dermatoloji", "Fizik Tedavi"
];

function Create_Doktor() {
    const navigate = useNavigate();

    // Adım kontrolü (0: Şifre İsteme, 1: Form Doldurma)
    const [step, setStep] = useState(0);
    const [secret, setSecret] = useState('');

    // Doktor DTO state
    const [formData, setFormData] = useState({
        tc: '',
        name: '',
        surname: '',
        password: '',
        eposta: '',
        alan: BRANSLAR[0]
    });

    const [loading, setLoading] = useState(false);
    const [error, setError] = useState(null);

    const handleSecretSubmit = (e) => {
        e.preventDefault();
        if (secret.trim() === '') {
            setError('Lütfen yetkilendirme şifresini girin.');
            return;
        }
        setError(null);
        setStep(1); // Şifre girildiyse forma geç
    };

    const handleChange = (e) => {
        const { name, value } = e.target;
        setFormData(prev => ({ ...prev, [name]: value }));
    };

    const handleFormSubmit = async (e) => {
        e.preventDefault();
        setLoading(true);
        setError(null);

        try {
            const response = await fetch('http://localhost:5160/api/Admin/Create Doktor', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'Secret': secret // Kullanıcının girdiği admin şifresi
                },
                body: JSON.stringify({
                    Tc: parseInt(formData.tc),
                    Name: formData.name,
                    Surname: formData.surname,
                    Password: formData.password,
                    Eposta: formData.eposta,
                    Alan: formData.alan
                })
            });

            if (!response.ok) {
                const errData = await response.json().catch(() => null);
                if (errData && errData.mesaj) {
                    throw new Error(errData.mesaj + (errData.hata ? ` - ${errData.hata}` : ''));
                } else if (typeof errData === 'string') {
                    throw new Error(errData);
                } else {
                    const textData = await response.text().catch(() => null);
                    throw new Error(textData || `İşlem başarısız`);
                }
            }

            // Başarılı olursa admin paneline dön
            navigate('/admin');

        } catch (err) {
            setError(err.message);
        } finally {
            setLoading(false);
        }
    };

    return (
        <div style={{ display: 'flex', flexDirection: 'column', alignItems: 'center', paddingTop: '30px' }}>
            <div className="fade-in" style={{
                background: 'var(--surface)',
                padding: '2rem',
                borderRadius: '16px',
                boxShadow: '0 8px 32px rgba(0, 0, 0, 0.2)',
                border: '1px solid rgba(255, 255, 255, 0.05)',
                width: '100%',
                maxWidth: '500px',
                marginTop: '10px'
            }}>
                {step === 0 ? (
                    // 1. Adım: Yetkilendirme Şifresi (Secret)
                    <div className="hasta-form-container" style={{ margin: 0, padding: 0, background: 'transparent', border: 'none', boxShadow: 'none' }}>
                        <h2 style={{ textAlign: 'center', marginBottom: '20px', color: 'var(--text-h)' }}>Admin Yetkilendirmesi</h2>
                        <p style={{ textAlign: 'center', color: 'var(--text)', marginBottom: '20px' }}>Devam etmek için işlem şifresini girmelisiniz.</p>
                        <form onSubmit={handleSecretSubmit}>
                            <div style={{ marginBottom: '15px' }}>
                                <label style={{ display: 'block', marginBottom: '8px', color: 'var(--text-h)', fontWeight: '500' }}>Admin Şifresi:</label>
                                <input
                                    type="password"
                                    value={secret}
                                    onChange={(e) => setSecret(e.target.value)}
                                    required
                                    style={{
                                        width: '100%',
                                        padding: '12px 16px',
                                        borderRadius: '8px',
                                        border: '1px solid var(--border)',
                                        background: 'var(--bg)',
                                        color: 'var(--text-h)',
                                        fontSize: '1rem',
                                        boxSizing: 'border-box'
                                    }}
                                />
                            </div>
                            {error && <div style={{ color: '#ef4444', marginBottom: '15px', fontWeight: '500' }}>{error}</div>}
                            <button type="submit" style={{
                                width: '100%',
                                padding: '12px',
                                borderRadius: '8px',
                                background: 'var(--accent)',
                                color: '#fff',
                                border: 'none',
                                fontSize: '1rem',
                                fontWeight: 'bold',
                                cursor: 'pointer',
                                transition: 'background 0.2s',
                                marginTop: '10px'
                            }}>
                                Doğrula
                            </button>
                        </form>
                    </div>
                ) : (
                    // 2. Adım: Doktor Ekleme Formu
                    <div className="hasta-form-container" style={{ margin: 0, padding: 0, background: 'transparent', border: 'none', boxShadow: 'none' }}>
                        <h2 style={{ textAlign: 'center', marginBottom: '20px', color: 'var(--text-h)' }}>Yeni Doktor Ekle</h2>
                        <form onSubmit={handleFormSubmit}>
                            <div style={{ marginBottom: '15px' }}>
                                <label style={{ display: 'block', marginBottom: '8px', color: 'var(--text-h)', fontWeight: '500' }}>TC Kimlik No:</label>
                                <input type="number" name="tc" value={formData.tc} onChange={handleChange} required style={inputStyle} />
                            </div>
                            <div style={{ marginBottom: '15px', display: 'flex', gap: '10px' }}>
                                <div style={{ flex: 1 }}>
                                    <label style={{ display: 'block', marginBottom: '8px', color: 'var(--text-h)', fontWeight: '500' }}>İsim:</label>
                                    <input type="text" name="name" value={formData.name} onChange={handleChange} required style={inputStyle} />
                                </div>
                                <div style={{ flex: 1 }}>
                                    <label style={{ display: 'block', marginBottom: '8px', color: 'var(--text-h)', fontWeight: '500' }}>Soyisim:</label>
                                    <input type="text" name="surname" value={formData.surname} onChange={handleChange} required style={inputStyle} />
                                </div>
                            </div>
                            <div style={{ marginBottom: '15px' }}>
                                <label style={{ display: 'block', marginBottom: '8px', color: 'var(--text-h)', fontWeight: '500' }}>Alan (Branş):</label>
                                <select name="alan" value={formData.alan} onChange={handleChange} required style={inputStyle}>
                                    {BRANSLAR.map(brans => (
                                        <option key={brans} value={brans}>{brans}</option>
                                    ))}
                                </select>
                            </div>
                            <div style={{ marginBottom: '15px' }}>
                                <label style={{ display: 'block', marginBottom: '8px', color: 'var(--text-h)', fontWeight: '500' }}>E-Posta:</label>
                                <input type="email" name="eposta" value={formData.eposta} onChange={handleChange} required style={inputStyle} />
                            </div>
                            <div style={{ marginBottom: '15px' }}>
                                <label style={{ display: 'block', marginBottom: '8px', color: 'var(--text-h)', fontWeight: '500' }}>Şifre:</label>
                                <input type="password" name="password" value={formData.password} onChange={handleChange} required style={inputStyle} />
                            </div>

                            {error && <div style={{ color: '#ef4444', marginBottom: '15px', fontWeight: '500' }}>{error}</div>}

                            <button type="submit" disabled={loading} style={{
                                width: '100%',
                                padding: '12px',
                                borderRadius: '8px',
                                background: loading ? '#9ca3af' : 'var(--accent)',
                                color: '#fff',
                                border: 'none',
                                fontSize: '1rem',
                                fontWeight: 'bold',
                                cursor: loading ? 'not-allowed' : 'pointer',
                                transition: 'background 0.2s',
                                marginTop: '10px'
                            }}>
                                {loading ? "Ekleniyor..." : "Ekle"}
                            </button>
                        </form>
                    </div>
                )}
            </div>
        </div>
    );
}

const inputStyle = {
    width: '100%',
    padding: '12px 16px',
    borderRadius: '8px',
    border: '1px solid var(--border)',
    background: 'var(--bg)',
    color: 'var(--text-h)',
    fontSize: '1rem',
    boxSizing: 'border-box'
};

export default Create_Doktor;
