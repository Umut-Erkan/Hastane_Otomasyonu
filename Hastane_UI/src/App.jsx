import { createBrowserRouter, RouterProvider, useNavigate } from 'react-router-dom';
import './App.css';
import HastaLanding from './Hasta/HastaLanding.jsx';
import HastaRandevuAl from './Hasta/Hasta_randevu_al.jsx';
import HastaRandevuGoster from './Hasta/Hasta_randevu_goster.jsx';
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
  return (
    <div className="page-wrapper">
      <div className="page-header" style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'flex-start' }}>
        <BackButton />
        <div>
          <GoToButton path="/hasta-panel/randevu-al" text="Randevu Al" />
          <GoToButton path="/hasta-panel/randevu-goster" text="Randevularım" />
        </div>
      </div>
      <div className="fade-in" style={{ padding: '40px 20px', textAlign: 'center', background: 'var(--surface)', borderRadius: '12px', marginTop: '20px' }}>
        <h2>Hasta Paneline Hoş Geldiniz</h2>
        <p style={{ color: 'var(--textSecondary)', marginTop: '10px' }}>Sağ üst köşedeki menüden işlemlerinizi gerçekleştirebilirsiniz.</p>
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
    path: "/doktor",
    element: <UnderConstructionPage title="Doktor" />,
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
