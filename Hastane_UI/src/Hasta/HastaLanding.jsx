
import HastaRegister from './Hasta_Register.jsx';
import HastaLogin from './HastaLogin.jsx';
import './HastaStyle/Hasta_add.css';

function HastaLanding() {
    return (
        <div style={{ display: 'flex', flexDirection: 'column', alignItems: 'center', paddingTop: '30px' }}>

            <div style={{
                display: 'flex',
                flexWrap: 'wrap',
                justifyContent: 'center',
                gap: '40px',
                width: '100%',
                maxWidth: '1000px',
                marginTop: '10px'
            }}>
                {/* Giriş Yap Bölümü */}
                <div className="fade-in" style={{
                    background: 'var(--surface)',
                    padding: '2rem',
                    borderRadius: '16px',
                    boxShadow: '0 8px 32px rgba(0, 0, 0, 0.2)',
                    border: '1px solid rgba(255, 255, 255, 0.05)',
                    flex: '1 1 400px'
                }}>
                    <HastaLogin />
                </div>

                {/* Kayıt Ol Bölümü */}
                <div className="fade-in" style={{
                    background: 'var(--surface)',
                    padding: '2rem',
                    borderRadius: '16px',
                    boxShadow: '0 8px 32px rgba(0, 0, 0, 0.2)',
                    border: '1px solid rgba(255, 255, 255, 0.05)',
                    flex: '1 1 400px'
                }}>
                    <HastaRegister />
                </div>
            </div>
        </div>
    );
}

export default HastaLanding;
