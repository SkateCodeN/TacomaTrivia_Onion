import { createBrowserRouter } from 'react-router-dom';
import { lazy } from 'react';
import Layout from '@widgets/Layout.jsx';
import Test from './Test';
import Home from '../../src/pages/Home';

import AuthUser from '@features/Form-Auth/AuthUser';
import LoginPage from '@features/Form-Auth/LoginPage';
const VenuesListPage = lazy(() => import('@features/venues/pages/VenuesListPage.jsx'));
const NotFound = () => <div style={{ padding: 24 }}>Not found</div>;

export const router = createBrowserRouter([
  {
    element: <Layout />,
    children: [
      { index: true, element: <Home /> },  // '/'
      { path: 'venues', element: <VenuesListPage /> },
      {path: 'login', element: <LoginPage />},

      {path:'/auth/user', element: <AuthUser />}
    ],
  },
  { path: '*', element: <NotFound /> }
]);
