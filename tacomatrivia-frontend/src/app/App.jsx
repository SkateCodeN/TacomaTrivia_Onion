import { Suspense } from 'react';
import { RouterProvider } from 'react-router-dom';
import Providers from '../providers/Providers.jsx';
import { router } from './routes.jsx';
import {useAuth0} from "@auth0/auth0-react"
export default function App() {

  const {loginWithRedirect, logout, getAccessTokenSilently, isAuthenticated, user} = useAuth0();

  return (
    <Providers>
      <Suspense fallback={<div style={{ padding: 24 }}>Loading…</div>}>
      {/* <button onClick={() => loginWithRedirect()}>Test Auth</button> */}
      <RouterProvider router={router} />
      </Suspense>
    </Providers>
  );
}
