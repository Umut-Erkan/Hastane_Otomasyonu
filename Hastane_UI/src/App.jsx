import { useState } from 'react'
import reactLogo from './assets/react.svg'
import viteLogo from './assets/vite.svg'
import heroImg from './assets/hero.png'
import './App.css'
import HastaEkle from './Hasta_add.jsx'

import Home from './Home.jsx'

function App() {
  const [currentPage, setCurrentPage] = useState('home')

  return (
    <>
      {currentPage === 'home' && <Home onNavigate={setCurrentPage} />}
      
      {currentPage === 'hasta' && (
        <div style={{ position: 'relative', width: '100%', minHeight: '100vh', display: 'flex', flexDirection: 'column' }}>
          <div style={{ padding: '20px', display: 'flex', justifyContent: 'flex-start' }}>
            <button 
              onClick={() => setCurrentPage('home')}
              style={{ padding: '10px 20px', borderRadius: '8px', cursor: 'pointer', border: 'none', background: 'var(--accent)', color: 'white', fontWeight: 'bold' }}
            >
              ← Ana Sayfaya Dön
            </button>
          </div>
          <HastaEkle />
        </div>
      )}

      {(currentPage === 'doktor' || currentPage === 'resepsiyonist') && (
        <div style={{ position: 'relative', width: '100%', minHeight: '100vh', display: 'flex', flexDirection: 'column', alignItems: 'center', justifyContent: 'center' }}>
          <h2>{currentPage === 'doktor' ? 'Doktor' : 'Resepsiyonist'} Sayfası Yapım Aşamasında</h2>
          <button 
            onClick={() => setCurrentPage('home')}
            style={{ padding: '10px 20px', borderRadius: '8px', cursor: 'pointer', border: 'none', background: 'var(--accent)', color: 'white', marginTop: '20px', fontWeight: 'bold' }}
          >
            ← Ana Sayfaya Dön
          </button>
        </div>
      )}
    </>
  )
}

export default App
