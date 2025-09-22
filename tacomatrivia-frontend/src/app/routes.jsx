import { createBrowserRouter } from 'react-router-dom';
import { lazy } from 'react';
import Layout from '@widgets/Layout.jsx';
import Test from './Test';

const VenuesListPage = lazy(() => import('@features/venues/pages/VenuesListPage.jsx'));
const NotFound = () => <div style={{ padding: 24 }}>Not found</div>;

export const router = createBrowserRouter([
  {
    element: <Layout />,
    children: [
      { index: true, element: <Test /> },  // '/'
      { path: 'venues', element: <VenuesListPage /> }
    ],
  },
  { path: '*', element: <NotFound /> }
]);
