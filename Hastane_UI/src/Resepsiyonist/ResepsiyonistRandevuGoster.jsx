import { useState, useEffect } from 'react';
import { useLocation } from 'react-router-dom';
import '../Hasta/HastaStyle/Hasta_add.css';

function ResepsiyonistRandevuGoster() {
    const location = useLocation();
    const userId = location.state?.userId;

    const [randevular, setRandevular] = useState([]);
    const [yukleniyor, setYukleniyor] = useState(true);
    const [hata, setHata] = useState(null);

    useEffect(() => {
        const fetchRandevular = async () => {
            try {
                const token = localStorage.getItem('resepsiyonistToken');

                if (token === null) {
                    throw new Error("Token bulunamadı. Lütfen önce giriş yapın.");
                }

                if (!userId) {
                    throw new Error("Doktor bilgisi bulunamadı.");
                }

                const response = await fetch(`http://localhost:5160/api/Doktor/RandevuGoster?userId=${userId}`, {
                    method: 'GET',
                    headers: {
                        'Content-Type': 'application/json',
                        "Authorization": `Bearer ${token}`,
                    }
                });

                if (response.status === 401) {
                    throw new Error(`Giriş yapmanız gerekiyor: HTTP ${response.status}`);
                }

                if (response.status === 403) {
                    throw new Error(`Yetkisiz erişim: HTTP ${response.status}`);
                }

                if (response.status === 404) {
                    setRandevular([]);
                    setYukleniyor(false);
                    return;
                }

                if (!response.ok) {
                    throw new Error(`Bir hata oluştu: HTTP ${response.status}`);
                }

                const veri = await response.json();
                console.log("Server Response:", veri);

                if (Array.isArray(veri)) {
                    setRandevular(veri);
                } else {
                    setRandevular([]);
                }

            } catch (err) {
                setHata(err.message);
            } finally {
                setYukleniyor(false);
            }
        };

        fetchRandevular();
    }, [userId]);

    return (
        <div className="hasta-form-container" style={{ maxWidth: '800px' }}>
            <div style={{
                display: 'flex',
                justifyContent: 'space-between',
                alignItems: 'center',
                marginBottom: '30px',
                borderBottom: '2px solid var(--accent-bg)',
                paddingBottom: '15px'
            }}>
                <h2 style={{ margin: 0, color: 'var(--text-h)', fontSize: '1.8rem' }}>Hekim Randevu Listesi</h2>
                <div style={{
                    backgroundColor: 'var(--accent-bg)',
                    color: 'var(--accent)',
                    padding: '5px 15px',
                    borderRadius: '20px',
                    fontSize: '0.9rem',
                    fontWeight: '600'
                }}>
                    {randevular.length} Randevu
                </div>
            </div>

            {yukleniyor && (
                <div style={{ display: 'flex', justifyContent: 'center', padding: '50px' }}>
                    <div className="loading-spinner">Yükleniyor...</div>
                </div>
            )}

            {hata && <div className="error-message" style={{ borderRadius: '12px' }}>⚠️ Hata: {hata}</div>}

            {!yukleniyor && randevular.length > 0 && (
                <div style={{ marginTop: '10px' }}>
                    <div style={{
                        display: 'grid',
                        gridTemplateColumns: 'repeat(auto-fill, minmax(300px, 1fr))',
                        gap: '20px'
                    }}>
                        {randevular.map((randevu, index) => (
                            <div
                                key={index}
                                style={{
                                    padding: '20px',
                                    border: '1px solid var(--border)',
                                    borderRadius: '16px',
                                    backgroundColor: 'var(--bg)',
                                    boxShadow: '0 4px 6px rgba(0,0,0,0.05)',
                                    transition: 'all 0.3s ease',
                                    display: 'flex',
                                    flexDirection: 'column',
                                    gap: '12px',
                                    position: 'relative',
                                    overflow: 'hidden'
                                }}
                                onMouseEnter={(e) => {
                                    e.currentTarget.style.transform = 'translateY(-5px)';
                                    e.currentTarget.style.boxShadow = '0 10px 20px rgba(0,0,0,0.1)';
                                    e.currentTarget.style.borderColor = 'var(--accent)';
                                }}
                                onMouseLeave={(e) => {
                                    e.currentTarget.style.transform = 'translateY(0)';
                                    e.currentTarget.style.boxShadow = '0 4px 6px rgba(0,0,0,0.05)';
                                    e.currentTarget.style.borderColor = 'var(--border)';
                                }}
                            >
                                <div style={{
                                    position: 'absolute',
                                    top: 0,
                                    left: 0,
                                    width: '4px',
                                    height: '100%',
                                    backgroundColor: 'var(--accent)'
                                }}></div>

                                <div style={{ display: 'flex', alignItems: 'center', gap: '10px' }}>
                                    <div style={{
                                        width: '40px',
                                        height: '40px',
                                        borderRadius: '10px',
                                        backgroundColor: 'var(--accent-bg)',
                                        display: 'flex',
                                        justifyContent: 'center',
                                        alignItems: 'center',
                                        fontSize: '1.2rem'
                                    }}>
                                        👤
                                    </div>
                                    <div>
                                        <div style={{ fontWeight: '700', color: 'var(--text-h)', fontSize: '1.1rem' }}>
                                            {randevu.hastaName} {randevu.hastaSurname}
                                        </div>
                                        <div style={{ fontSize: '0.8rem', color: '#718096' }}>Hasta Profili</div>
                                    </div>
                                </div>

                                <div style={{
                                    backgroundColor: 'rgba(0,0,0,0.02)',
                                    padding: '15px',
                                    borderRadius: '12px',
                                    fontSize: '0.95rem',
                                    color: 'var(--text)',
                                    border: '1px dashed var(--border)',
                                    lineHeight: '1.5',
                                    position: 'relative'
                                }}>
                                    <span style={{
                                        display: 'block',
                                        fontWeight: '700',
                                        color: 'var(--text-h)',
                                        fontSize: '0.75rem',
                                        textTransform: 'uppercase',
                                        letterSpacing: '0.05em',
                                        marginBottom: '5px',
                                        opacity: 0.7
                                    }}>
                                        📋 Hasta Şikayeti
                                    </span>
                                    {randevu.hastaŞikayet || "Belirtilen bir şikayet bulunmuyor."}
                                </div>

                                <div style={{
                                    display: 'flex',
                                    justifyContent: 'space-between',
                                    marginTop: 'auto',
                                    paddingTop: '10px',
                                    borderTop: '1px solid #edf2f7',
                                    fontSize: '0.85rem'
                                }}>
                                    <div style={{ display: 'flex', alignItems: 'center', gap: '5px' }}>
                                        📅 {randevu.tarih}
                                    </div>
                                    <div style={{ display: 'flex', alignItems: 'center', gap: '5px', fontWeight: '600', color: 'var(--accent)' }}>
                                        ⏰ {randevu.saat}
                                    </div>
                                </div>
                            </div>
                        ))}
                    </div>
                </div>
            )}

            {!yukleniyor && !hata && randevular.length === 0 && (
                <div style={{
                    textAlign: 'center',
                    padding: '40px',
                    backgroundColor: '#f8fafc',
                    borderRadius: '20px',
                    border: '2px dashed #e2e8f0',
                    marginTop: '20px'
                }}>
                    <div style={{ fontSize: '3rem', marginBottom: '10px' }}>📁</div>
                    <div style={{ color: '#64748b', fontWeight: '600', fontSize: '1.1rem' }}>Henüz randevu bulunmamaktadır.</div>
                    <div style={{ color: '#94a3b8', fontSize: '0.9rem', marginTop: '5px' }}>Doktorun müsaitliği için diğer tarihleri kontrol edebilirsiniz.</div>
                </div>
            )}
        </div>
    );
}

export default ResepsiyonistRandevuGoster;
