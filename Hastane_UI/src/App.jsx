import { createBrowserRouter, RouterProvider, useNavigate } from 'react-router-dom';
import './App.css';
import HastaLanding from './Hasta/HastaLanding.jsx';
import HastaRandevuAl from './Hasta/Hasta_randevu_al.jsx';
import HastaRandevuGoster from './Hasta/Hasta_randevu_goster.jsx';
import HastaReceteGoster from './Hasta/Hasta_recete_goster.jsx';
import DoktorLogin from './Doktor/DoktorLogin.jsx';
import DoktorRandevuGoster from './Doktor/Doktor_randevu_goster.jsx';
import DoktorTedaviYaz from './Doktor/Doktor_tedavi_yaz.jsx';
import Home from './Home.jsx';

// Sayfalar arası geçişte kullanılacak geri dön butonu bileşeni
function BackButton() {
  const navigate = useNavigate();
  return (
    <button
      onClick={() => navigate('/')}
      className="back-button"
    >
      ← Ana Sayfaya Dön
    </button>
  );
}

function GoToButton({ path, text }) {
  const navigate = useNavigate();
  return (
    <button
      onClick={() => navigate(path)}
      style={{
        padding: '10px 20px', borderRadius: '8px', cursor: 'pointer', border: '1px solid var(--accent)',
        background: 'transparent', color: 'var(--accent)', fontWeight: 'bold', margin: '0 10px'
      }}
    >
      {text}
    </button>
  );
}

// Hasta Sayfası Wrapper
function HastaPage() {
  console.log("hasta-panel");
  const navigate = useNavigate();

  // Şık Navigasyon Kartı (Dashboard stili)
  const ActionCard = ({ title, desc, path, icon }) => (
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
      onMouseOver={(e) => { e.currentTarget.style.transform = 'translateY(-5px)'; e.currentTarget.style.boxShadow = '0 8px 30px rgba(37, 99, 235, 0.2)'; }}
      onMouseOut={(e) => { e.currentTarget.style.transform = 'none'; e.currentTarget.style.boxShadow = '0 4px 20px rgba(0,0,0,0.1)'; }}
    >
      <div style={{ fontSize: '3.5rem' }}>{icon}</div>
      <h3 style={{ margin: 0, color: 'var(--text)', fontSize: '1.4rem' }}>{title}</h3>
      <p style={{ margin: 0, color: 'var(--textSecondary)', textAlign: 'center', fontSize: '0.95rem' }}>{desc}</p>
    </div>
  );

  return (
    <div className="page-wrapper fade-in">
      <div className="page-header" style={{ marginBottom: '0px' }}>
        <BackButton />
      </div>

      <div style={{ textAlign: 'center', marginBottom: '50px' }}>
        <h2 style={{ fontSize: '2.5rem', fontWeight: 'bold' }}>Hasta Paneline Hoş Geldiniz</h2>
        <p style={{ color: 'var(--textSecondary)', marginTop: '10px', fontSize: '1.2rem' }}>Aşağıdaki menüden işlemlerinizi hızlıca gerçekleştirebilirsiniz.</p>
      </div>

      <div style={{ display: 'grid', gridTemplateColumns: 'repeat(auto-fit, minmax(280px, 1fr))', gap: '30px', maxWidth: '1000px', margin: '0 auto' }}>
        <ActionCard
          title="Randevu Al"
          desc="Uygun hekimlerimizden kolayca yeni bir randevu oluşturun."
          path="/hasta-panel/randevu-al"

        />
        <ActionCard
          title="Randevu Göster"
          desc="Yaklaşan ve geçmiş hastane randevularınızı görüntüleyin."
          path="/hasta-panel/randevu-goster"

        />
        <ActionCard
          title="Reçete Göster"
          desc="Size yazılan reçeteleri ve ilaç detaylarınızı inceleyin."
          path="/hasta-panel/recete-goster"

        />
      </div>
    </div>
  );
}

// Randevu Al Wrapper
function HastaRandevuAlPage() {
  return (
    <div className="page-wrapper">
      <div className="page-header">
        <BackButton />
      </div>
      <HastaRandevuAl />
    </div>
  );
}

// Randevu Görüntüle Wrapper
function HastaRandevuGosterPage() {
  return (
    <div className="page-wrapper">
      <div className="page-header">
        <BackButton />
      </div>
      <HastaRandevuGoster />
    </div>
  );
}

// Reçete Görüntüle Wrapper
function HastaReceteGosterPage() {
  return (
    <div className="page-wrapper">
      <div className="page-header">
        <BackButton />
      </div>
      <HastaReceteGoster />
    </div>
  );
}

// Doktor Randevu Görüntüle Wrapper
function DoktorRandevuGosterPage() {
  return (
    <div className="page-wrapper">
      <div className="page-header">
        <BackButton />
      </div>
      <DoktorRandevuGoster />
    </div>
  );
}

// Doktor Tedavi Yaz Wrapper
function DoktorTedaviYazPage() {
  return (
    <div className="page-wrapper">
      <div className="page-header">
        <BackButton />
      </div>
      <DoktorTedaviYaz />
    </div>
  );
}

// Yapım Aşamasında Sayfası Wrapper
function UnderConstructionPage({ title }) {
  return (
    <div className="under-construction-wrapper">
      <h2>{title} Sayfası Yapım Aşamasında</h2>
      <BackButton />
    </div>
  );
}

const router = createBrowserRouter([
  {
    path: "/",
    element: <Home />,
  },
  {
    path: "/hasta",
    element: <HastaLanding />,
  },
  {
    path: "/hasta-panel",
    element: <HastaPage />,
  },
  {
    path: "/hasta-panel/randevu-al",
    element: <HastaRandevuAlPage />,
  },
  {
    path: "/hasta-panel/randevu-goster",
    element: <HastaRandevuGosterPage />,
  },
  {
    path: "/hasta-panel/recete-goster",
    element: <HastaReceteGosterPage />,
  },


  {
    path: "/doktor",
    element: <DoktorLogin />,
  },
  {
    path: "/doktor-panel/randevu-goster",
    element: <DoktorRandevuGosterPage />,
  },
  {
    path: "/doktor-panel/tedavi-yaz",
    element: <DoktorTedaviYazPage />,
  },
  {
    path: "/resepsiyonist",
    element: <UnderConstructionPage title="Resepsiyonist" />,
  },
]);

function App() {
  return <RouterProvider router={router} />;
}

export default App;
