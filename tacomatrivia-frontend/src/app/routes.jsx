import { createBrowserRouter } from 'react-router-dom';
import { lazy } from 'react';
import Layout from '@widgets/Layout.jsx';
import Home from '@features/UI/pages/Home.jsx'
import TeamRecordsListPage from '@features/TeamRecords/pages/TeamRecordsListPage';

const VenuesListPage = lazy(() => import('@features/venues/pages/VenuesListPage.jsx'));

const UsersListPage = lazy( () => import ('@features/users/components/UsersTable'))
const NotFound = () => <div style={{ padding: 24 }}>Not found</div>;

export const router = createBrowserRouter([
  {
    element: <Layout />,
    children: [
      { index: true, element: <Home /> },  // '/'
      { path: 'venues', element: <VenuesListPage /> },
      { path: 'users', element: <UsersListPage /> },
      {path: 'teamrecords', element: <TeamRecordsListPage />}
    ],
  },
  { path: '*', element: <NotFound /> }
]);
