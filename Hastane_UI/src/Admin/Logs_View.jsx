import React, { useState } from 'react';

function Logs_View() {
    const [step, setStep] = useState(0);
    const [secret, setSecret] = useState('');
    const [logs, setLogs] = useState([]);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState(null);

    const handleSecretSubmit = async (e) => {
        e.preventDefault();
        if (secret.trim() === '') {
            setError('Lütfen yetkilendirme şifresini girin.');
            return;
        }
        setLoading(true);
        setError(null);

        try {
            const response = await fetch('http://localhost:5160/api/Admin/Logs', {
                method: 'GET',
                headers: {
                    'Content-Type': 'application/json',
                    'Secret': secret
                }
            });

            if (!response.ok) {
                if (response.status === 401 || response.status === 403) {
                    throw new Error('Yetkilendirme başarısız. Admin şifresi hatalı.');
                }
                const errData = await response.json().catch(() => null);
                throw new Error(errData?.mesaj || `Hata: ${response.status}`);
            }

            const data = await response.json();
            setLogs(data);
            setStep(1);
        } catch (err) {
            setError(err.message);
        } finally {
            setLoading(false);
        }
    };

    return (
        <div style={{ display: 'flex', flexDirection: 'column', alignItems: 'center', paddingTop: '30px' }}>

            {step === 0 ? (
                // ── 1. ADIM: Şifre Ekranı ──────────────────────────────────────────
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
                    <div className="hasta-form-container" style={{ margin: 0, padding: 0, background: 'transparent', border: 'none', boxShadow: 'none' }}>
                        <div style={{ textAlign: 'center', marginBottom: '24px' }}>
                            <div style={{ fontSize: '3rem', marginBottom: '10px' }}>🛡️</div>
                            <h2 style={{ color: 'var(--text-h)', margin: 0 }}>Admin Yetkilendirmesi</h2>
                            <p style={{ color: 'var(--text)', marginTop: '8px' }}>
                                Sistem loglarını görüntülemek için işlem şifresini girmelisiniz.
                            </p>
                        </div>

                        <form onSubmit={handleSecretSubmit}>
                            <div style={{ marginBottom: '15px' }}>
                                <label style={{ display: 'block', marginBottom: '8px', color: 'var(--text-h)', fontWeight: '500' }}>
                                    Admin Şifresi:
                                </label>
                                <input
                                    type="password"
                                    value={secret}
                                    onChange={(e) => setSecret(e.target.value)}
                                    required
                                    placeholder="••••••••"
                                    style={inputStyle}
                                />
                            </div>

                            {error && (
                                <div style={{ color: '#ef4444', marginBottom: '15px', fontWeight: '500', padding: '10px', background: 'rgba(239,68,68,0.1)', borderRadius: '8px' }}>
                                    ⚠️ {error}
                                </div>
                            )}

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
                                {loading ? 'Doğrulanıyor...' : 'Logları Getir'}
                            </button>
                        </form>
                    </div>
                </div>
            ) : (
                // ── 2. ADIM: Log Tablosu ────────────────────────────────────────────
                <div className="fade-in" style={{ width: '100%', maxWidth: '1200px', padding: '0 1rem' }}>

                    {/* Başlık & İstatistik */}
                    <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: '24px', flexWrap: 'wrap', gap: '12px' }}>
                        <div>
                            <h2 style={{ color: 'var(--text-h)', margin: 0, fontSize: '1.8rem' }}>
                                📋 Sistem Logları
                            </h2>
                            <p style={{ color: 'var(--text)', margin: '4px 0 0', fontSize: '0.9rem' }}>
                                Toplam <strong style={{ color: 'var(--accent)' }}>{logs.length}</strong> kayıt
                            </p>
                        </div>
                    </div>

                    {/* Tablo */}
                    <div style={{
                        background: 'var(--surface)',
                        borderRadius: '16px',
                        border: '1px solid rgba(255,255,255,0.05)',
                        boxShadow: '0 8px 32px rgba(0,0,0,0.2)',
                        overflow: 'hidden'
                    }}>
                        <div style={{ overflowX: 'auto' }}>
                            <table style={{ width: '100%', borderCollapse: 'collapse', fontSize: '0.9rem' }}>
                                <thead>
                                    <tr style={{ background: 'rgba(139,92,246,0.15)' }}>
                                        {['#', 'Kullanıcı ID', 'Rol', 'Servis / Endpoint', 'IP Adresi', 'Tarayıcı', 'Zaman'].map(h => (
                                            <th key={h} style={thStyle}>{h}</th>
                                        ))}
                                    </tr>
                                </thead>
                                <tbody>
                                    {logs.length === 0 ? (
                                        <tr>
                                            <td colSpan={7} style={{ textAlign: 'center', padding: '40px', color: 'var(--text)' }}>
                                                Kayıtlı log bulunamadı.
                                            </td>
                                        </tr>
                                    ) : (
                                        logs.map((log, i) => (
                                            <tr
                                                key={log.logId}
                                                style={{
                                                    borderTop: '1px solid rgba(255,255,255,0.05)',
                                                    background: i % 2 === 0 ? 'transparent' : 'rgba(255,255,255,0.02)',
                                                    transition: 'background 0.15s'
                                                }}
                                                onMouseOver={e => e.currentTarget.style.background = 'rgba(139,92,246,0.08)'}
                                                onMouseOut={e => e.currentTarget.style.background = i % 2 === 0 ? 'transparent' : 'rgba(255,255,255,0.02)'}
                                            >
                                                <td style={tdStyle}>{log.logId}</td>
                                                <td style={{ ...tdStyle, textAlign: 'center' }}>
                                                    {log.userıd === 0
                                                        ? <span style={{ color: '#9ca3af', fontStyle: 'italic' }}>Anonim</span>
                                                        : <span style={{ color: 'var(--accent)', fontWeight: '600' }}>{log.userıd}</span>
                                                    }
                                                </td>
                                                <td style={{ ...tdStyle, textAlign: 'center' }}>
                                                    <RoleBadge role={log.role} />
                                                </td>
                                                <td style={{ ...tdStyle, fontFamily: 'monospace', fontSize: '0.82rem', color: 'var(--text-h)' }}>
                                                    {log.serviceName}
                                                </td>
                                                <td style={{ ...tdStyle, fontFamily: 'monospace', fontSize: '0.82rem' }}>
                                                    {log.ipAddress ?? <span style={{ color: '#9ca3af' }}>—</span>}
                                                </td>
                                                <td style={{ ...tdStyle, maxWidth: '180px', overflow: 'hidden', textOverflow: 'ellipsis', whiteSpace: 'nowrap' }}
                                                    title={log.browserInfo}>
                                                    {log.browserInfo}
                                                </td>
                                                <td style={{ ...tdStyle, whiteSpace: 'nowrap', color: '#a78bfa', fontSize: '0.82rem' }}>
                                                    {log.time ? new Date(log.time).toLocaleString('tr-TR') : '—'}
                                                </td>
                                            </tr>
                                        ))
                                    )}
                                </tbody>
                            </table>
                        </div>
                    </div>

                    {/* Yenile butonu */}
                    <div style={{ textAlign: 'center', marginTop: '20px' }}>
                        <button
                            onClick={() => { setStep(0); setSecret(''); setLogs([]); }}
                            style={{
                                padding: '10px 24px',
                                borderRadius: '8px',
                                background: 'transparent',
                                color: 'var(--accent)',
                                border: '1px solid var(--accent)',
                                fontSize: '0.95rem',
                                cursor: 'pointer',
                                transition: 'all 0.2s'
                            }}
                            onMouseOver={e => { e.currentTarget.style.background = 'var(--accent)'; e.currentTarget.style.color = '#fff'; }}
                            onMouseOut={e => { e.currentTarget.style.background = 'transparent'; e.currentTarget.style.color = 'var(--accent)'; }}
                        >
                            🔄 Yeniden Yükle
                        </button>
                    </div>
                </div>
            )}
        </div>
    );
}

// Rol renk badge'i
function RoleBadge({ role }) {
    const colors = {
        'Admin': { bg: 'rgba(239,68,68,0.15)', text: '#ef4444' },
        'Doktor': { bg: 'rgba(59,130,246,0.15)', text: '#60a5fa' },
        'Resepsiyonist': { bg: 'rgba(16,185,129,0.15)', text: '#34d399' },
        'Hasta': { bg: 'rgba(245,158,11,0.15)', text: '#fbbf24' },
    };
    const c = colors[role] ?? { bg: 'rgba(156,163,175,0.15)', text: '#9ca3af' };
    return (
        <span style={{
            background: c.bg,
            color: c.text,
            padding: '3px 10px',
            borderRadius: '20px',
            fontSize: '0.78rem',
            fontWeight: '600',
            whiteSpace: 'nowrap'
        }}>
            {role ?? 'Unknown'}
        </span>
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
    boxSizing: 'border-box',
    outline: 'none'
};

const thStyle = {
    padding: '14px 16px',
    textAlign: 'left',
    color: 'var(--text-h)',
    fontWeight: '600',
    fontSize: '0.85rem',
    textTransform: 'uppercase',
    letterSpacing: '0.05em',
    whiteSpace: 'nowrap'
};

const tdStyle = {
    padding: '12px 16px',
    color: 'var(--text)',
    verticalAlign: 'middle'
};

export default Logs_View;
