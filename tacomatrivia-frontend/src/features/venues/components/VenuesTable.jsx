import { useEffect, useMemo, useState } from 'react';
import { Table, Group, TextInput, Button, Loader, Center, Badge } from '@mantine/core';
import { IconSearch, IconRefresh } from '@tabler/icons-react';
import RowsPerPageSelect from '@shared/ui/RowsPerPageSelect.jsx';
import useDebounce from '@shared/hooks/useDebounce.js';
import { venuesApi } from '../api/venuesApi.js';

export default function VenuesTable() {
  const [rows, setRows] = useState([]);
  const [total, setTotal] = useState(undefined);

  const [q, setQ] = useState('');
  const dq = useDebounce(q, 350);

  const [page, setPage] = useState(1);
  const [pageSize, setPageSize] = useState(25);
  const [loading, setLoading] = useState(false);
  const [err, setErr] = useState('');

  const hasNextHeuristic = rows.length === pageSize && (total === undefined);
  const pages = useMemo(() => total ? Math.max(1, Math.ceil(total / pageSize)) : undefined, [total, pageSize]);

  const fetchData = async () => {
    setLoading(true); setErr('');
    try {
      const { items, total } = await venuesApi.list({ q: dq, page, pageSize });
      setRows(items || []);
      setTotal(total);
    } catch (e) {
      setErr(e.message || 'Failed to load');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => { fetchData(); }, [dq, page, pageSize]);

  return (
    <div>
      <Group justify="space-between" mb="sm" wrap="wrap">
        <Group>
          <TextInput
            leftSection={<IconSearch size={16} />}
            placeholder="Search venues…"
            value={q}
            onChange={(e) => { setPage(1); setQ(e.target.value); }}
          />
          <Button variant="light" onClick={fetchData} leftSection={<IconRefresh size={16} />}>
            Refresh
          </Button>
        </Group>
        <RowsPerPageSelect value={pageSize} onChange={(n) => { setPage(1); setPageSize(n); }} />
      </Group>

      <div style={{ position: 'relative' }}>
        {loading && (
          <Center style={{ position: 'absolute', inset: 0, background: 'rgba(255,255,255,0.5)', zIndex: 1 }}>
            <Loader />
          </Center>
        )}

        <Table striped highlightOnHover withTableBorder withColumnBorders>
          <Table.Thead>
            <Table.Tr>
              <Table.Th>Name</Table.Th>
              <Table.Th>Address</Table.Th>
              <Table.Th>Open</Table.Th>
            </Table.Tr>
          </Table.Thead>
          <Table.Tbody>
            {err && (
              <Table.Tr>
                <Table.Td colSpan={3} style={{ color: 'var(--mantine-color-red-6)' }}>
                  Error: {err}
                </Table.Td>
              </Table.Tr>
            )}
            {!err && !loading && rows.length === 0 && (
              <Table.Tr><Table.Td colSpan={3}>No venues.</Table.Td></Table.Tr>
            )}
            {rows.map((v) => (
              <Table.Tr key={v.id}>
                <Table.Td>{v.name}</Table.Td>
                <Table.Td>{v.address}</Table.Td>
                <Table.Td>
                  <Badge color={v.allowsPets || v.isOpen ? 'green' : 'gray'}>
                    {(v.isOpen ?? v.allowsPets) ? 'Open' : 'Closed'}
                  </Badge>
                </Table.Td>
              </Table.Tr>
            ))}
          </Table.Tbody>
        </Table>
      </div>

      <Group justify="space-between" mt="sm" wrap="wrap">
        <div style={{ fontSize: 12, opacity: 0.8 }}>
          Page {page}{total ? ` / ${pages}` : ''}{total ? ` • ${total} total` : ''}
        </div>
        <Group>
          <Button variant="default" onClick={() => setPage((p) => Math.max(1, p - 1))} disabled={page === 1}>
            Prev
          </Button>
          <Button
            onClick={() => setPage((p) => p + 1)}
            disabled={total ? page >= pages : !hasNextHeuristic}
          >
            Next
          </Button>
        </Group>
      </Group>
    </div>
  );
}
