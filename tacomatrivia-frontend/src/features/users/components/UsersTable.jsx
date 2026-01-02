import { useEffect, useMemo, useState } from 'react';
import { Table, Group, TextInput, Button, Loader, Center, Badge } from '@mantine/core';
import { IconSearch, IconRefresh } from '@tabler/icons-react';

import RowsPerPageSelect from '@shared/ui/RowsPerPageSelect.jsx';
import useDebounce from '@shared/hooks/useDebounce.js';

import { usersApi } from '../api/usersApi.js';

// import DayConverter from '../helper functions/DayConverter.js';
import CreateUserDialog from './CreateUserDialog.jsx';
// import OpenDeleteRequest from './OpenDeleteRequest.jsx';
import EditUserDialog from './EditUserDialog.jsx'

export default function UsersTable() {
  const [rows, setRows] = useState([]);
  const [total, setTotal] = useState(undefined);

  // To handle the opening of the modal
  const [open, setOpen] = useState(false);
  // Handle the open and clode of the delete dialog
  const [openDelete, setOpenDelete] = useState(false);

  const [openEdit, setOpenEdit] = useState(false);
  const [rowData, setRowData] = useState([]);

  const [q, setQ] = useState('');
  const dq = useDebounce(q, 350);

  const [page, setPage] = useState(1);
  const [pageSize, setPageSize] = useState(25);
  const [loading, setLoading] = useState(false);
  const [err, setErr] = useState('');

  const hasNextHeuristic = rows.length === pageSize && (total === undefined);
  const pages = useMemo(() => total ? Math.max(1, Math.ceil(total / pageSize)) : undefined, [total, pageSize]);

  const fetchData = async () => {
    setLoading(true); 
    setErr('');
    try {
      const { items, total } = await usersApi.list({ q: dq, page, pageSize });
      setRows(items || []);
      setTotal(total);
    } catch (e) {
      setErr(e.message || 'Failed to load');
    } finally {
      setLoading(false);
    }
  };

  // When we create a new user, this function is called
  // on our child component, this will run
  const onUserCreated = () => {
    //re-hydrate data
    alert("User was created")
    fetchData();
  }

  const handleDelete = async (id) => {
    try {
      
      await usersApi.delete(id);

    }
    catch (error) {
      setErr(error?.message || 'Failed to delete us');
    }
    finally{
      onUserCreated();
    }
  }
  useEffect(() => { fetchData(); }, [dq, page, pageSize]);

  return (
    <div>
      <Group justify="space-between" mb="sm" wrap="wrap">
        <Button onClick={() => setOpen(true)}>
          Add New
        </Button>
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
              <Table.Th>Email</Table.Th>
              <Table.Th>Phone</Table.Th>
              <Table.Th>Role</Table.Th>
              <Table.Th>Action</Table.Th>
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
                <Table.Td>{v.email}</Table.Td>
                <Table.Td>{v.phone}</Table.Td>
                <Table.Td>{v.role}</Table.Td>
                <Table.Td>
                  <div style={{ display: "flex", justifyContent: "space-around" }}>
                    <Button
                      color='yellow'
                      onClick={() => {
                        setRowData(v);
                        setOpenEdit(true)
                      }}
                    >
                      Edit
                    </Button>
                    <Button
                      color='red'
                      onClick={() => handleDelete(v.id)}
                    >
                      Delete
                    </Button>
                  </div>

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

      <CreateUserDialog
        opened={open}
        onClose={() => setOpen(false)}
        onCreated={onUserCreated}
      />

      <EditUserDialog
        opened={openEdit}
        onClose={() => setOpenEdit(false)}
        rowData={rowData}
        onEdited={onUserCreated}
      />
      {/*
      <OpenDeleteRequest
        opened={openDelete}
        onClose={() => setOpenDelete(false)}
      />

      */}
    </div>
  );
}
