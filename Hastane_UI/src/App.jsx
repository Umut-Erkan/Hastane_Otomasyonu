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
import ResepsiyonistLogin from './Resepsiyonist/ResepsiyonistLogin.jsx';
import ResepsiyonistDashboard from './Resepsiyonist/ResepsiyonistDashboard.jsx';
import ResepsiyonistRandevuGoster from './Resepsiyonist/ResepsiyonistRandevuGoster.jsx';

// Sol üst: bir önceki sayfaya geri döner
function BackButton() {
  const navigate = useNavigate();
  return (
    <button
      onClick={() => navigate(-1)}
      className="nav-btn nav-btn-back"
      title="Geri"
    >
      ← Geri
    </button>
  );
}

// Sağ üst: ana sayfaya gider
function HomeButton() {
  const navigate = useNavigate();
  return (
    <button
      onClick={() => navigate('/')}
      className="nav-btn nav-btn-home"
      title="Ana Sayfa"
    >
      🏠 Ana Sayfa
    </button>
  );
}

// İki butonlu navigasyon çubuğu
function NavBar() {
  return (
    <div className="page-nav-bar">
      <BackButton />
      <HomeButton />
    </div>
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
      <NavBar />

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
      <NavBar />
      <HastaRandevuAl />
    </div>
  );
}

// Randevu Görüntüle Wrapper
function HastaRandevuGosterPage() {
  return (
    <div className="page-wrapper">
      <NavBar />
      <HastaRandevuGoster />
    </div>
  );
}

// Reçete Görüntüle Wrapper
function HastaReceteGosterPage() {
  return (
    <div className="page-wrapper">
      <NavBar />
      <HastaReceteGoster />
    </div>
  );
}

// Doktor Login Wrapper
function DoktorLoginPage() {
  return (
    <div className="page-wrapper">
      <div className="page-nav-bar">
        <span />
        <HomeButton />
      </div>
      <DoktorLogin />
    </div>
  );
}

// Doktor Randevu Görüntüle Wrapper
function DoktorRandevuGosterPage() {
  return (
    <div className="page-wrapper">
      <NavBar />
      <DoktorRandevuGoster />
    </div>
  );
}

// Doktor Tedavi Yaz Wrapper
function DoktorTedaviYazPage() {
  return (
    <div className="page-wrapper">
      <NavBar />
      <DoktorTedaviYaz />
    </div>
  );
}

// Resepsiyonist Login Wrapper
function ResepsiyonistLoginPage() {
  return (
    <div className="page-wrapper">
      <div className="page-nav-bar">
        <span />
        <HomeButton />
      </div>
      <ResepsiyonistLogin />
    </div>
  );
}

// Resepsiyonist Dashboard Wrapper
function ResepsiyonistDashboardPage() {
  return (
    <div className="page-wrapper">
      <NavBar />
      <ResepsiyonistDashboard />
    </div>
  );
}

// Resepsiyonist Randevu Göster Wrapper
function ResepsiyonistRandevuGosterPage() {
  return (
    <div className="page-wrapper">
      <NavBar />
      <ResepsiyonistRandevuGoster />
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

// Hasta Landing Wrapper (sadece Home butonu)
function HastaLandingPage() {
  return (
    <div className="page-wrapper">
      <div className="page-nav-bar">
        <span />
        <HomeButton />
      </div>
      <HastaLanding />
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
    element: <HastaLandingPage />,
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
    element: <DoktorLoginPage />,
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
    element: <ResepsiyonistLoginPage />,
  },
  {
    path: "/resepsiyonist-panel/dashboard",
    element: <ResepsiyonistDashboardPage />,
  },
  {
    path: "/resepsiyonist-panel/randevu-goster",
    element: <ResepsiyonistRandevuGosterPage />,
  },
]);

function App() {
  return <RouterProvider router={router} />;
}

export default App;
