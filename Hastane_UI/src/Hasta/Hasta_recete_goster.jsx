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
    <div className="hasta-form-container fade-in" style={{ maxWidth: '800px', margin: '0 auto', padding: '20px' }}>
      <h2 style={{ textAlign: 'center', marginBottom: '20px', color: 'var(--primary, #007bff)' }}>Tedavilerim ve Reçetelerim</h2>

      {yukleniyor && (
        <div style={{ textAlign: 'center', padding: '20px', color: '#666' }}>
          <div style={{ display: 'inline-block', width: '30px', height: '30px', border: '3px solid rgba(0,0,0,0.1)', borderRadius: '50%', borderTopColor: 'var(--primary, #007bff)', animation: 'spin 1s ease-in-out infinite' }}></div>
          <style>{`@keyframes spin { to { transform: rotate(360deg); } }`}</style>
          <p style={{ marginTop: '10px' }}>Yükleniyor...</p>
        </div>
      )}

      {hata && (
        <div className="error-message" style={{ color: '#721c24', backgroundColor: '#f8d7da', border: '1px solid #f5c6cb', padding: '15px', borderRadius: '8px', textAlign: 'center', marginBottom: '20px' }}>
          <strong>Hata:</strong> {hata}
        </div>
      )}

      {!yukleniyor && receteler.length > 0 && (
        <div style={{ display: 'grid', gap: '20px' }}>
          {receteler.map((veri, index) => (
            <div key={index} style={{
              padding: '20px',
              background: 'var(--surface, #ffffff)',
              border: '1px solid #e0e0e0',
              boxShadow: '0 4px 6px rgba(0,0,0,0.05)',
              borderRadius: '12px',
              transition: 'transform 0.2s ease, box-shadow 0.2s ease',
            }}
              onMouseEnter={(e) => { e.currentTarget.style.transform = 'translateY(-3px)'; e.currentTarget.style.boxShadow = '0 6px 12px rgba(0,0,0,0.1)'; }}
              onMouseLeave={(e) => { e.currentTarget.style.transform = 'translateY(0)'; e.currentTarget.style.boxShadow = '0 4px 6px rgba(0,0,0,0.05)'; }}
            >
              <div style={{ paddingBottom: '5px' }}>
                <h3 style={{ margin: '0 0 12px 0', color: '#333', display: 'flex', alignItems: 'center', gap: '8px' }}>
                  <span style={{ fontSize: '1.2rem' }}>🩺</span>
                  Tanı: <span style={{ fontWeight: 'normal', color: '#555' }}>{veri.tanı || "Belirtilmemiş"}</span>
                </h3>
                <div style={{ display: 'flex', flexDirection: 'column', gap: '8px', fontSize: '1rem', color: '#555', backgroundColor: '#f8f9fa', padding: '15px', borderRadius: '8px', borderLeft: '4px solid #17a2b8' }}>
                  <div>
                    <strong>Hasta:</strong> {veri.hastaName}
                  </div>
                  <div>
                    <strong>Doktor:</strong> {veri.doktorName} {veri.doktorSurname}
                  </div>
                </div>
              </div>

            </div>
          ))}
        </div>
      )}

      {!yukleniyor && !hata && receteler.length === 0 && (
        <div style={{ textAlign: 'center', marginTop: '40px', padding: '50px 20px', backgroundColor: '#f8f9fa', borderRadius: '12px', border: '1px dashed #ced4da' }}>
          <div style={{ fontSize: '3rem', marginBottom: '15px' }}>📝</div>
          <h3 style={{ color: '#495057', marginBottom: '10px' }}>Kayıtlı Tedaviniz Yok</h3>
          <p style={{ color: '#6c757d' }}>Sisteme girilmiş bir tedaviniz bulunmuyor.</p>
        </div>
      )}
    </div>
  );
}

export default Hasta_recete_goster;
