import React from 'react';
import { createBrowserRouter, RouterProvider, useNavigate } from 'react-router-dom';
import './App.css';
import HastaLanding from './Hasta/HastaLanding.jsx';
import HastaRandevuAl from './Hasta/Hasta_randevu_al.jsx';
import HastaRandevuGoster from './Hasta/Hasta_randevu_goster.jsx';
import HastaReceteGoster from './Hasta/Hasta_recete_goster.jsx';
import HastaLogout from './Hasta/Hasta_Logout.jsx';
import HastaProfile from './Hasta/Hasta_Profile.jsx';
import DoktorLogin from './Doktor/DoktorLogin.jsx';
import DoktorRandevuGoster from './Doktor/Doktor_randevu_goster.jsx';
import DoktorTedaviYaz from './Doktor/Doktor_tedavi_yaz.jsx';
import Home from './Home.jsx';
import ResepsiyonistLogin from './Resepsiyonist/ResepsiyonistLogin.jsx';
import ResepsiyonistDashboard from './Resepsiyonist/ResepsiyonistDashboard.jsx';
import ResepsiyonistRandevuGoster from './Resepsiyonist/ResepsiyonistRandevuGoster.jsx';
import AdminPanel from './Admin/Admin_Panel.jsx';

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

// Admin Paneli Sayfası Wrapper
function AdminPanelPage() {
  return (
    <div className="page-wrapper fade-in">
      <div className="page-nav-bar">
        <span />
        <HomeButton />
      </div>
      <AdminPanel />
    </div>
  );
}

// Hasta Panel Sayfası – içeriği Hasta_Profile.jsx'te
function HastaPage() {
  return (
    <div className="page-wrapper">
      <NavBar />
      <HastaProfile />
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
    path: "/admin",
    element: <AdminPanelPage />,
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
    path: "/hasta-panel/logout",
    element: <div className="page-wrapper"><NavBar /><HastaLogout /></div>,
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
