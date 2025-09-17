import { Outlet, NavLink } from 'react-router-dom';

export default function Layout() {
  return (
    <div style={{ maxWidth: 1100, margin: '0 auto', padding: 16 }}>
      <nav style={{ display: 'flex', gap: 12, padding: '12px 0', borderBottom: '1px solid #eee' }}>
        <NavLink to="/" end>Home</NavLink>
        <NavLink to="/venues">Venues</NavLink>
      </nav>
      <main>
        <Outlet />
      </main>
    </div>
  );
}
