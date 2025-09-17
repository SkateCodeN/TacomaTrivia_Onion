import { Title, Stack } from '@mantine/core';
import VenuesTable from '../components/VenuesTable.jsx';

export default function VenuesListPage() {
  return (
    <Stack>
      <Title order={2}>Venues</Title>
      <VenuesTable />
    </Stack>
  );
}
