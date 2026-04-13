import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';

function Hasta_Logout() {
    const navigate = useNavigate();
    const [durum, setDurum] = useState('cikis'); // 'cikis' | 'hata'
    const [hataMsg, setHataMsg] = useState('');

    useEffect(() => {
        const logout = async () => {
            const token = localStorage.getItem('hastaToken');

            try {
                await fetch('http://localhost:5160/api/Hasta/Logout', {
                    method: 'DELETE',
                    headers: {
                        'Authorization': `Bearer ${token}`,
                        'Content-Type': 'application/json',
                    },
                });
            } catch (err) {
                // API hatası olsa bile token'ı temizle ve yönlendir
                console.warn('Logout API hatası:', err.message);
            } finally {
                localStorage.removeItem('hastaToken');
                // En az 1.8 saniye beklet ki kullanıcı mesajı görsün
                setTimeout(() => {
                    navigate('/hasta');
                }, 1800);
            }
        };

        logout();
    }, [navigate]);

    return (
        <div style={{
            display: 'flex',
            flexDirection: 'column',
            alignItems: 'center',
            justifyContent: 'center',
            minHeight: '60vh',
            gap: '24px',
        }}>
            {/* Dönen halka animasyonu */}
            <div style={{
                width: '60px',
                height: '60px',
                border: '5px solid var(--accent-bg, rgba(192,132,252,0.2))',
                borderTop: '5px solid var(--accent, #c084fc)',
                borderRadius: '50%',
                animation: 'spin 0.9s linear infinite',
            }} />

            <p style={{
                fontSize: '1.4rem',
                fontWeight: '600',
                color: 'var(--text-h)',
                letterSpacing: '0.5px',
            }}>
                Çıkış yapılıyor<span style={{ animation: 'blink 1.2s steps(3, end) infinite' }}>...</span>
            </p>

            <style>{`
                @keyframes spin {
                    to { transform: rotate(360deg); }
                }
                @keyframes blink {
                    0%, 100% { opacity: 1; }
                    50% { opacity: 0.2; }
                }
            `}</style>
        </div>
    );
}

export default Hasta_Logout;
