import { Outlet, NavLink } from 'react-router-dom';
import AppNavbar from '../../src/components/AppNavBar';

export default function Layout() {
  return (
    <div style={{ maxWidth: 1100, margin: '0 auto', padding: 16 }}>
  
      <AppNavbar />
      <main>
        <Outlet />
      </main>
    </div>
  );
}
