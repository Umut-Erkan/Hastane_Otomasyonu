import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';

// ---------- Yardımcı: Navigasyon Kartı ----------
function ActionCard({ title, desc, path, icon, navigate }) {
    return (
        <div
            onClick={() => navigate(path)}
            style={{
                background: 'var(--surface)',
                padding: '30px',
                borderRadius: '16px',
                cursor: 'pointer',
                border: '1px solid var(--accent)',
                boxShadow: '0 4px 20px rgba(0,0,0,0.1)',
                display: 'flex',
                flexDirection: 'column',
                alignItems: 'center',
                gap: '15px',
                transition: 'transform 0.3s ease, box-shadow 0.3s ease',
            }}
            onMouseOver={e => {
                e.currentTarget.style.transform = 'translateY(-5px)';
                e.currentTarget.style.boxShadow = '0 8px 30px rgba(170, 59, 255, 0.25)';
            }}
            onMouseOut={e => {
                e.currentTarget.style.transform = 'none';
                e.currentTarget.style.boxShadow = '0 4px 20px rgba(0,0,0,0.1)';
            }}
        >
            <div style={{ fontSize: '3.5rem' }}>{icon}</div>
            <h3 style={{ margin: 0, color: 'var(--text-h)', fontSize: '1.4rem' }}>{title}</h3>
            <p style={{ margin: 0, color: 'var(--text)', textAlign: 'center', fontSize: '0.95rem' }}>{desc}</p>
        </div>
    );
}

// ---------- Yardımcı: Profil Avatarı & Açılır Menü ----------
function ProfilAvatar({ navigate }) {
    const [menuAcik, setMenuAcik] = useState(false);

    const MENU_SECENEKLERI = [
        { label: 'Randevu Göster', path: '/hasta-panel/randevu-goster' },
        { label: 'Reçete Göster', path: '/hasta-panel/recete-goster' },
    ];

    return (
        <div style={{ display: 'flex', justifyContent: 'center', marginBottom: '32px', position: 'relative' }}>
            <div style={{ display: 'flex', flexDirection: 'column', alignItems: 'center', gap: '12px' }}>

                {/* Avatar dairesi – tıklanınca menü açılır */}
                <div
                    onClick={() => setMenuAcik(prev => !prev)}
                    title="Profil menüsü"
                    style={{
                        width: '90px',
                        height: '90px',
                        borderRadius: '50%',
                        background: 'linear-gradient(135deg, var(--accent, #aa3bff) 0%, #7c3aed 100%)',
                        display: 'flex',
                        alignItems: 'center',
                        justifyContent: 'center',
                        fontSize: '2.8rem',
                        cursor: 'pointer',
                        boxShadow: '0 4px 20px rgba(170,59,255,0.4)',
                        border: '3px solid var(--accent, #aa3bff)',
                        transition: 'transform 0.2s ease, box-shadow 0.2s ease',
                        userSelect: 'none',
                    }}
                    onMouseOver={e => {
                        e.currentTarget.style.transform = 'scale(1.06)';
                        e.currentTarget.style.boxShadow = '0 8px 30px rgba(170,59,255,0.55)';
                    }}
                    onMouseOut={e => {
                        e.currentTarget.style.transform = 'scale(1)';
                        e.currentTarget.style.boxShadow = '0 4px 20px rgba(170,59,255,0.4)';
                    }}
                >
                    🧑
                </div>

                <span style={{ fontWeight: '600', fontSize: '1rem', color: 'var(--text-h)' }}>
                    Hasta Profili
                </span>

                {/* Açılır dropdown menü */}
                {menuAcik && (
                    <div style={{
                        position: 'absolute',
                        top: '120px',
                        background: 'var(--bg)',
                        border: '1px solid var(--accent)',
                        borderRadius: '12px',
                        boxShadow: '0 8px 32px rgba(0,0,0,0.18)',
                        overflow: 'hidden',
                        zIndex: 100,
                        minWidth: '200px',
                        animation: 'fadeSlideIn 0.18s ease',
                    }}>
                        <style>{`
                            @keyframes fadeSlideIn {
                                from { opacity: 0; transform: translateY(-8px); }
                                to   { opacity: 1; transform: translateY(0); }
                            }
                        `}</style>

                        {/* Sayfa yönlendirme seçenekleri */}
                        {MENU_SECENEKLERI.map(({ label, path }) => (
                            <div
                                key={path}
                                onClick={() => { setMenuAcik(false); navigate(path); }}
                                style={{
                                    padding: '12px 20px',
                                    cursor: 'pointer',
                                    fontSize: '0.95rem',
                                    color: 'var(--text-h)',
                                    transition: 'background 0.15s',
                                    textAlign: 'left',
                                }}
                                onMouseOver={e => e.currentTarget.style.background = 'var(--accent-bg)'}
                                onMouseOut={e => e.currentTarget.style.background = 'transparent'}
                            >
                                {label}
                            </div>
                        ))}

                        {/* Ayırıcı çizgi */}
                        <div style={{ height: '1px', background: 'var(--border)' }} />

                        {/* Çıkış seçeneği */}
                        <div
                            onClick={() => { setMenuAcik(false); navigate('/hasta-panel/logout'); }}
                            style={{
                                padding: '12px 20px',
                                cursor: 'pointer',
                                fontSize: '0.95rem',
                                color: '#ef4444',
                                transition: 'background 0.15s',
                                textAlign: 'left',
                            }}
                            onMouseOver={e => e.currentTarget.style.background = 'rgba(239,68,68,0.08)'}
                            onMouseOut={e => e.currentTarget.style.background = 'transparent'}
                        >
                            Çıkış Yap
                        </div>
                    </div>
                )}
            </div>
        </div>
    );
}

// ---------- Ana Bileşen ----------
function Hasta_Profile() {
    const navigate = useNavigate();

    return (
        <div className="page-wrapper fade-in">

            {/* Profil avatarı ve açılır menü */}
            <ProfilAvatar navigate={navigate} />

            {/* Başlık */}
            <div style={{ textAlign: 'center', marginBottom: '50px' }}>
                <h2 style={{ fontSize: '2.5rem', fontWeight: 'bold' }}>
                    Hasta Paneline Hoş Geldiniz
                </h2>
                <p style={{ color: 'var(--text)', marginTop: '10px', fontSize: '1.2rem' }}>
                    Aşağıdaki menüden işlemlerinizi hızlıca gerçekleştirebilirsiniz.
                </p>
            </div>

            {/* Aksiyon kartları */}
            <div style={{
                display: 'grid',
                gridTemplateColumns: 'repeat(auto-fit, minmax(280px, 1fr))',
                gap: '30px',
                maxWidth: '1000px',
                margin: '0 auto',
            }}>
                <ActionCard
                    title="Randevu Al"
                    desc="Uygun hekimlerimizden kolayca yeni bir randevu oluşturun."
                    path="/hasta-panel/randevu-al"
                    icon="📋"
                    navigate={navigate}
                />
            </div>
        </div>
    );
}

export default Hasta_Profile;
