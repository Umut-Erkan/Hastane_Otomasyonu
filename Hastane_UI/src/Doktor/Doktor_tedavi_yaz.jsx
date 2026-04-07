import React, { useState, useEffect } from 'react';
import { useLocation, useNavigate } from 'react-router-dom';
import '../Hasta/HastaStyle/Hasta_add.css';

function DoktorTedaviYaz() {
    const location = useLocation();
    const navigate = useNavigate();

    const [hastaId] = useState(location.state?.hastaId);
    const [randevuId] = useState(location.state?.randevuId);
    useEffect(() => {
        console.log("Hasta ID:", hastaId);
        console.log("Randevu ID:", randevuId);
    }, []);

    const [tani, setTani] = useState('');
    const [kullanim, setKullanim] = useState('');
    const [yukleniyor, setYukleniyor] = useState(false);
    const [hata, setHata] = useState(null);
    const [basari, setBasari] = useState(null);

    // İlaçlar için dinamik state listesi
    const [ilaclarListesi, setIlaclarListesi] = useState([{ isim: '', adet: 1 }]);
    const [sistemIlaclari, setSistemIlaclari] = useState([]);

    useEffect(() => {
        const fetchIlaclar = async () => {
            try {
                const token = localStorage.getItem('doktorToken');
                const response = await fetch('http://localhost:5160/api/Doktor/GetIlaclar', {
                    headers: { 'Authorization': `Bearer ${token}` }
                });
                if (response.ok) {
                    const data = await response.json();
                    setSistemIlaclari(data);
                } else {
                    console.error("İlaç listesi alınamadı, durum:", response.status);
                }
            } catch (error) {
                console.error("İlaçlar getirilirken hata oluştu:", error);
            }
        };

        fetchIlaclar();
    }, []);

    const handleIlacDegistir = (index, field, value) => {
        const yeniIlaclar = [...ilaclarListesi];
        yeniIlaclar[index][field] = value;
        setIlaclarListesi(yeniIlaclar);
    };

    const handleIlacEkle = () => {
        setIlaclarListesi([...ilaclarListesi, { isim: '', adet: 1 }]);
    };

    const handleIlacSil = (index) => {
        const yeniIlaclar = ilaclarListesi.filter((_, i) => i !== index);
        setIlaclarListesi(yeniIlaclar);
    };

    const handleSubmit = async (e) => {
        e.preventDefault();

        if (!hastaId) {
            setHata("Hasta ID bulunamadı. Lütfen randevular sayfasına dönün.");
            return;
        }

        setYukleniyor(true);
        setHata(null);
        setBasari(null);

        try {
            const token = localStorage.getItem('doktorToken');

            const gecerliIlaclar = ilaclarListesi.filter(ilac => ilac.isim.trim() !== '');

            const payload = {
                tanı: tani,
                hastaID: hastaId,
                kullanım: kullanim,
                ilacAdet: gecerliIlaclar.map(ilac => parseInt(ilac.adet, 10)),
                ilaclar: gecerliIlaclar.map(ilac => ilac.isim.trim())
            };

            const response = await fetch('http://localhost:5160/api/Doktor/Tedavi%20yaz', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${token}`
                },
                body: JSON.stringify(payload)
            });

            if (!response.ok) {
                const errVeri = await response.json();
                throw new Error(errVeri.mesaj || `Hata oluştu: ${response.status}`);
            }

            const veri = await response.json();
            setBasari(veri.mesaj || "Tedavi başarıyla yazıldı. Yönlendiriliyorsunuz...");

            // Başarılı olduğunda 2 saniye sonra randevularım sayfasına dön
            setTimeout(() => {
                navigate('/doktor-panel/randevu-goster');
            }, 2000);

        } catch (err) {
            setHata(err.message);
        } finally {
            setYukleniyor(false);
        }
    };

    return (
        <div className="hasta-form-container" style={{ maxWidth: '600px', margin: '0 auto' }}>
            <h2 style={{ textAlign: 'center' }}>Tedavi Yazma Sayfası</h2>
            <p style={{ textAlign: 'center', marginBottom: '20px' }}>Bu alanda hastaya ait reçete ve tedavi detayları tanımlanacaktır.</p>

            {hata && <div className="error-message" style={{ color: 'red', marginBottom: '15px' }}>{hata}</div>}
            {basari && <div className="success-message" style={{ color: 'green', marginBottom: '15px', fontWeight: 'bold' }}>{basari}</div>}

            <form onSubmit={handleSubmit} style={{ display: 'flex', flexDirection: 'column', gap: '15px' }}>
                <div className="form-group" style={{ display: 'flex', flexDirection: 'column', textAlign: 'left' }}>
                    <label htmlFor="tani" style={{ fontWeight: 'bold' }}>Tanı:</label>
                    <textarea
                        id="tani"
                        value={tani}
                        onChange={(e) => setTani(e.target.value)}
                        required
                        rows={3}
                        style={{ padding: '8px', borderRadius: '4px', border: '1px solid #ccc' }}
                    />
                </div>

                <div className="form-group" style={{ display: 'flex', flexDirection: 'column', textAlign: 'left' }}>
                    <label htmlFor="kullanim" style={{ fontWeight: 'bold' }}>İlaç/Tedavi Kullanım Şekli:</label>
                    <textarea
                        id="kullanim"
                        value={kullanim}
                        onChange={(e) => setKullanim(e.target.value)}
                        required
                        rows={3}
                        style={{ padding: '8px', borderRadius: '4px', border: '1px solid #ccc' }}
                    />
                </div>

                <div className="form-group" style={{ display: 'flex', flexDirection: 'column', textAlign: 'left' }}>
                    <label style={{ fontWeight: 'bold' }}>Yazılacak İlaç ve Adeti:</label>
                    {ilaclarListesi.map((ilac, index) => (
                        <div key={index} style={{ display: 'flex', gap: '10px', marginTop: '5px' }}>
                            <select
                                value={ilac.isim}
                                onChange={(e) => handleIlacDegistir(index, 'isim', e.target.value)}
                                style={{ flex: 2, padding: '8px', borderRadius: '4px', border: '1px solid #ccc' }}
                            >
                                <option value="" disabled>İlaç Seçiniz...</option>
                                {sistemIlaclari.map((sItem, idx) => (
                                    <option key={sItem.ilacId || idx} value={sItem.ilacName || sItem.IlacName}>
                                        {sItem.ilacName || sItem.IlacName} ({sItem.kullanımAlanı || sItem.KullanımAlanı})
                                    </option>
                                ))}
                            </select>
                            <input
                                type="number"
                                min="1"
                                placeholder="Adet"
                                value={ilac.adet}
                                onChange={(e) => handleIlacDegistir(index, 'adet', e.target.value)}
                                style={{ flex: 1, padding: '8px', borderRadius: '4px', border: '1px solid #ccc' }}
                            />
                            {ilaclarListesi.length > 1 && (
                                <button
                                    type="button"
                                    onClick={() => handleIlacSil(index)}
                                    style={{ padding: '8px 12px', backgroundColor: '#dc3545', color: 'white', border: 'none', borderRadius: '4px', cursor: 'pointer', fontWeight: 'bold' }}
                                    title="İlacı Sil"
                                >
                                    X
                                </button>
                            )}
                        </div>
                    ))}
                    <button
                        type="button"
                        onClick={handleIlacEkle}
                        style={{ padding: '8px 15px', backgroundColor: '#17a2b8', color: 'white', border: 'none', borderRadius: '4px', cursor: 'pointer', fontWeight: 'bold', alignSelf: 'flex-start', marginTop: '10px' }}
                    >
                        + Yeni İlaç Ekle
                    </button>
                    <small style={{ color: '#666', marginTop: '10px', display: 'block', fontSize: '0.85em' }}>Not: Sadece sistemde kayıtlı ilaçlar seçilebilir.</small>
                </div>

                <div style={{ display: 'flex', gap: '10px', marginTop: '10px' }}>
                    <button
                        type="submit"
                        disabled={yukleniyor}
                        style={{
                            padding: '10px 15px',
                            backgroundColor: '#28a745',
                            color: 'white',
                            border: 'none',
                            borderRadius: '4px',
                            cursor: yukleniyor ? 'not-allowed' : 'pointer',
                            fontWeight: 'bold',
                            marginTop: '10px'
                        }}
                    >
                        {yukleniyor ? 'Kaydediliyor...' : 'Tedaviyi Kaydet'}
                    </button>

                    <button
                        type="button"
                        onClick={() => navigate('/doktor-panel/randevu-goster')}
                        style={{
                            padding: '10px 15px',
                            backgroundColor: '#6c757d',
                            color: 'white',
                            border: 'none',
                            borderRadius: '4px',
                            cursor: 'pointer',
                            fontWeight: 'bold'
                        }}
                    >
                        İptal ve Geri Dön
                    </button>
                </div>
            </form>
        </div>
    );
}

export default DoktorTedaviYaz;
