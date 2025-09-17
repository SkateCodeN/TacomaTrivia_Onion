import { Suspense } from 'react';
import { RouterProvider } from 'react-router-dom';
import Providers from '../providers/Providers.jsx';
import { router } from './routes.jsx';

export default function App() {
  return (
    <Providers>
      <Suspense fallback={<div style={{ padding: 24 }}>Loading…</div>}>
        <RouterProvider router={router} />
      </Suspense>
    </Providers>
  );
}
