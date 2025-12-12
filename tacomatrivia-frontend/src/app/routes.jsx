import { createBrowserRouter } from 'react-router-dom';
import { lazy } from 'react';
import Layout from '@widgets/Layout.jsx';
import Home from '@features/UI/pages/Home.jsx'

import AuthHome from '@features/auth/HomePage'
const VenuesListPage = lazy(() => import('@features/venues/pages/VenuesListPage.jsx'));
const NotFound = () => <div style={{ padding: 24 }}>Not found</div>;

export const router = createBrowserRouter([
  {
    element: <Layout />,
    children: [
      { index: true, element: <Home /> },  // '/'
      { path: 'venues', element: <VenuesListPage /> },
      { path: '/auth/callback', element: <AuthHome />}
    ],
  },
  { path: '*', element: <NotFound /> }
]);
