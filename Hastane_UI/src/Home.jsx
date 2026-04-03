import { useNavigate } from 'react-router-dom';
import './Home.css';

const homeCards = [
  { id: 'doktor', title: 'Doktor', icon: '🩺', description: 'Doktor paneline giriş yapın', color: '#3b82f6' },
  { id: 'hasta', title: 'Hasta', icon: '🏥', description: 'Hasta işlemlerini yönetin', color: '#10b981' },
  { id: 'resepsiyonist', title: 'Resepsiyonist', icon: '📝', description: 'Kayıt ve randevu işlemleri', color: '#f59e0b' },
];

export default function Home() {
  const navigate = useNavigate();

  return (
    <div className="home-container">
      <div className="home-content">
        <h1 className="home-title">Hastane Yönetim Sistemi</h1>
        <p className="home-subtitle">Lütfen işlem yapmak istediğiniz modülü seçin</p>
        
        <div className="cards-grid">
          {homeCards.map((card) => (
            <button 
              key={card.id} 
              className={`home-card card-${card.id}`}
              onClick={() => navigate(`/${card.id}`)}
            >
              <div className="card-icon" style={{ backgroundColor: card.color + '20', color: card.color }}>
                {card.icon}
              </div>
              <h2>{card.title}</h2>
              <p>{card.description}</p>
              <div className="card-glow" style={{ backgroundColor: card.color }}></div>
            </button>
          ))}
        </div>
      </div>
    </div>
  );
}
