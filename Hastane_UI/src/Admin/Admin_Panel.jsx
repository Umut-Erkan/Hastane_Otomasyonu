import React from 'react';
import { useNavigate } from 'react-router-dom';

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
                e.currentTarget.style.boxShadow = '0 8px 30px rgba(139, 92, 246, 0.25)';
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

function Admin_Panel() {
    const navigate = useNavigate();

    return (
        <div className="fade-in">
            <div style={{ textAlign: 'center', marginBottom: '50px' }}>
                <h2 style={{ fontSize: '2.5rem', fontWeight: 'bold' }}>
                    Admin Paneline Hoş Geldiniz
                </h2>
                <p style={{ color: 'var(--text)', marginTop: '10px', fontSize: '1.2rem' }}>
                    Lütfen yapmak istediğiniz işlemi seçin.
                </p>
            </div>

            <div style={{
                display: 'grid',
                gridTemplateColumns: 'repeat(auto-fit, minmax(280px, 1fr))',
                gap: '30px',
                maxWidth: '800px',
                margin: '0 auto',
            }}>
                <ActionCard
                    title="Doktor Ekle"
                    desc="Sisteme yeni bir doktor kaydı oluşturun."
                    path="/admin/doktor-ekle"
                    navigate={navigate}
                />
                <ActionCard
                    title="Resepsiyonist Ekle"
                    desc="Sisteme yeni kayıt ve kabul personeli ekleyin."
                    path="/admin/resepsiyonist-ekle"
                    navigate={navigate}
                />
            </div>
        </div>
    );
}

export default Admin_Panel;
