import { useState, useEffect } from 'react';
import './HastaStyle/Hasta_add.css';

function Hasta_recete_goster() {
  const [receteler, setReceteler] = useState([]);
  const [yukleniyor, setYukleniyor] = useState(true);
  const [hata, setHata] = useState(null);

  useEffect(() => {
    const fetchReceteler = async () => {
      try {
        const token = localStorage.getItem('hastaToken');

        if (token === null) {
          throw new Error("Token bulunamadı. Lütfen giriş yapın.");
        }

        const response = await fetch(`http://localhost:5160/api/Hasta/ReceteGoster`, {
          method: 'GET',
          headers: {
            'Content-Type': 'application/json',
            "Authorization": `Bearer ${token}`,
          }
        });

        console.log("Sunucudan Gelen Reçete Verisi:", response);

        if (response.status === 401) {
          throw new Error(`Oturumunuz süresi dolmuş veya giriş yapmanız gerekiyor (HTTP ${response.status})`);
        }

        if (response.status === 403) {
          throw new Error(`Bu sayfayı görüntüleme yetkiniz yok (HTTP ${response.status})`);
        }

        if (!response.ok) {
          // Eger backend 404 gibi bir sonuc dönerse hatayı yakala
          const errText = await response.text();
          throw new Error(`Sunucu hatası: ${errText} (HTTP ${response.status})`);
        }

        const veri = await response.json();
        console.log("Sunucudan Gelen Reçete Verisi:", veri);

        const isArray = Array.isArray(veri);
        if ((isArray && veri.length === 0) || (!isArray && veri.message)) {
          setReceteler([]);
        } else {
          setReceteler(isArray ? veri : [veri]);
        }

      } catch (err) {
        setHata(err.message);
      } finally {
        setYukleniyor(false);
      }
    };

    fetchReceteler();
  }, []);

  return (
    <div className="fade-in" style={{
      maxWidth: '900px',
      margin: '0 auto',
      padding: '40px 20px',
      minHeight: '80vh'
    }}>
      <div style={{ textAlign: 'center', marginBottom: '40px' }}>
        <h1 style={{ margin: '0 0 10px 0', color: 'var(--text-h)' }}>Tedaviler ve Reçeteler</h1>
        <p style={{ color: 'var(--text)', fontSize: '1.1rem' }}>Geçmiş tedavilerinizi ve reçete edilen ilaçlarınızı buradan takip edebilirsiniz.</p>
      </div>

      {yukleniyor && (
        <div style={{ textAlign: 'center', padding: '50px' }}>
          <div style={{
            display: 'inline-block',
            width: '40px',
            height: '40px',
            border: '3px solid var(--accent-bg)',
            borderRadius: '50%',
            borderTopColor: 'var(--accent)',
            animation: 'spin 1s linear infinite'
          }}></div>
          <style>{`@keyframes spin { to { transform: rotate(360deg); } }`}</style>
          <p style={{ marginTop: '15px', color: 'var(--text)' }}>Bilgileriniz getiriliyor...</p>
        </div>
      )}

      {hata && (
        <div className="error-message" style={{ margin: '20px auto', maxWidth: '500px' }}>
          <strong>Hata:</strong> {hata}
        </div>
      )}

      {!yukleniyor && receteler.length > 0 && (
        <div style={{
          display: 'grid',
          gridTemplateColumns: '1fr',
          gap: '24px'
        }}>
          {receteler.map((veri, index) => (
            <div key={index} style={{
              padding: '24px',
              background: 'var(--bg)',
              border: '1px solid var(--border)',
              boxShadow: 'var(--shadow)',
              borderRadius: '16px',
              transition: 'all 0.3s ease',
              textAlign: 'left'
            }}
              onMouseEnter={(e) => {
                e.currentTarget.style.transform = 'translateY(-4px)';
                e.currentTarget.style.borderColor = 'var(--accent-border)';
              }}
              onMouseLeave={(e) => {
                e.currentTarget.style.transform = 'translateY(0)';
                e.currentTarget.style.borderColor = 'var(--border)';
              }}
            >
              <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'flex-start', marginBottom: '20px', borderBottom: '1px solid var(--border)', paddingBottom: '15px' }}>
                <div>
                  <h2 style={{ margin: '0 0 5px 0', fontSize: '1.4rem' }}>
                    <span style={{ marginRight: '10px' }}>🩺</span>
                    {veri.tanı || "Tanı Belirtilmemiş"}
                  </h2>
                  <div style={{ color: 'var(--text)', fontSize: '0.9rem' }}>
                    <strong>Doktor:</strong> {veri.doktorName} {veri.doktorSurname}
                  </div>
                </div>
                <div style={{
                  backgroundColor: 'var(--accent-bg)',
                  color: 'var(--accent)',
                  padding: '6px 14px',
                  borderRadius: '20px',
                  fontSize: '0.85rem',
                  fontWeight: '600'
                }}>
                  Tamamlandı
                </div>
              </div>

              {veri.ilaclar && veri.ilaclar.length > 0 ? (
                <div>
                  <h3 style={{ fontSize: '1.1rem', marginBottom: '15px', display: 'flex', alignItems: 'center', gap: '8px' }}>
                    <span style={{ fontSize: '1.2rem' }}>💊</span> Reçete Edilen İlaçlar
                  </h3>
                  <div style={{ display: 'grid', gridTemplateColumns: 'repeat(auto-fill, minmax(250px, 1fr))', gap: '12px' }}>
                    {veri.ilaclar.map((ilac, i) => (
                      <div key={i} style={{
                        display: 'flex',
                        justifyContent: 'space-between',
                        alignItems: 'center',
                        backgroundColor: 'var(--accent-bg)',
                        padding: '12px 16px',
                        borderRadius: '10px',
                        border: '1px solid var(--accent-border)'
                      }}>
                        <span style={{ color: 'var(--text-h)', fontWeight: '700', fontSize: '1rem' }}>
                          {ilac.ilacName}
                        </span>
                        <span style={{
                          backgroundColor: 'var(--accent)',
                          color: '#fff',
                          padding: '2px 10px',
                          borderRadius: '6px',
                          fontSize: '0.8rem',
                          fontWeight: 'bold'
                        }}>
                          {ilac.adet} Adet
                        </span>
                      </div>
                    ))}
                  </div>
                </div>
              ) : (
                <div style={{ padding: '10px', color: 'var(--text)', fontStyle: 'italic', fontSize: '0.9rem' }}>
                  Bu tedavi için reçete edilmiş ilaç bulunmamaktadır.
                </div>
              )}
            </div>
          ))}
        </div>
      )}

      {!yukleniyor && !hata && receteler.length === 0 && (
        <div style={{
          textAlign: 'center',
          marginTop: '60px',
          padding: '60px 20px',
          backgroundColor: 'var(--accent-bg)',
          borderRadius: '20px',
          border: '2px dashed var(--accent-border)'
        }}>
          <div style={{ fontSize: '4rem', marginBottom: '20px' }}>📄</div>
          <h2 style={{ color: 'var(--text-h)' }}>Henüz Reçeteniz Bulunmuyor</h2>
          <p style={{ color: 'var(--text)', maxWidth: '400px', margin: '0 auto' }}>
            Doktorunuz tarafından sisteme girilen herhangi bir reçete veya tedavi kaydı bulunmamaktadır.
          </p>
        </div>
      )}
    </div>
  );
}

export default Hasta_recete_goster;
