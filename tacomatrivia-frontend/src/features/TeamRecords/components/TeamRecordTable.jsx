import { useEffect, useState, useMemo } from "react";
import { Table, Group, TextInput, Button, Loader, Center, Typography } from '@mantine/core';
import { IconSearch, IconRefresh } from '@tabler/icons-react';

import RowsPerPageSelect from "@shared/ui/RowsPerPageSelect";
import useDebounce from "@shared/hooks/useDebounce";
import { teamRecordsApi } from "../api/teamRecordsApi";
export default function TeamRecordTable() {
    // Rows is rows of data from DB
    const [rows, setRows] = useState([]);
    const [total, setTotal] = useState(undefined);
    //set the page size to 25
    const [pageSize, setPageSize] = useState(25);
    //we set our current page to num 1
    const [page, setPage] = useState(1);
    const [loading, setLoading] = useState(false);
    // To showcase error
    const[err, setErr] = useState('');
    //modal state to open/close

    // we calculate how many total pages, if there is a change, then 
    // it will get memoized.
    const pages = useMemo(() => total ? Math.max(1, Math.ceil(total / pageSize)) : undefined, [total, pageSize]);

    // we calculate if there is a next available page.
    const hasNextHeuristic = rows.length === pageSize && (total === undefined);

    // q is for our search bar, we use a debounce to slow down 
    // as we type in the query
    const [query, setQuery] = useState('');
    const debouncedQuery = useDebounce(query, 350);


    //we fetch our data from teamrecords
    const fetchData = async () => {
        setLoading(true);
        setErr('');
        try {
            const { items, total } = await teamRecordsApi.list({ q: debouncedQuery, page, pageSize });
            setRows(items || []);
            setTotal(total);
        }
        catch (e) {
            setErr(e.message || "Unable to load data from team records")
        }
        finally {
            setLoading(false);
        }
    };

    //Once we mount this component useEffect runs when debounceQuery, page, and pageSize
    useEffect(() => { fetchData(); }, [debouncedQuery, page, pageSize]);

    return (
        <div>
            <Group justify="space-between" mb="sm" wrap="wrap">
                {/* We have the add button here */}

                <Group>
                    <TextInput
                        leftSection={<IconSearch size={16} />}
                        placeholder="Search Team Records..."
                        values={query}
                        onChange={(e) => { setPage(1); setQuery(e.target.value); }}
                    />

                    <Button variant="light" onClick={fetchData} leftSection={<IconRefresh size={16} />}>
                        Refresh
                    </Button>
                </Group>
                <RowsPerPageSelect value={pageSize} onChange={(n) => { setPage(1); setPageSize(n); }} />
            </Group>

            {/* Loading component */}
            <div style={{ position: 'relative' }}>
                {loading && (
                    <Center style={{ position: 'absolute', inset: 0, background: 'rgba(255,255,255,0.5)', zIndex: 1 }}>
                        <Loader />
                    </Center>
                )}

                {/* Table component */}
                <Table striped highlightOnHover withTableBorder withColumnBorders>
                    <Table.Thead>
                        <Table.Tr>
                            <Table.Th>Id</Table.Th>
                            <Table.Th>Date</Table.Th>
                            <Table.Th>Venue</Table.Th>
                            <Table.Th>Team</Table.Th>
                            <Table.Th>Placed</Table.Th>
                            <Table.Th>Points</Table.Th>
                            <Table.Th>Name Used</Table.Th>
                        </Table.Tr>
                    </Table.Thead>

                    <Table.Tbody>
                        {
                            err && (
                                <Table.Tr>
                                    <Table.Td colSpan={3} style={{ color: 'var(--mantine-color-red-6)' }}>
                                        Error: {err}
                                    </Table.Td>
                                </Table.Tr>
                            )
                        }
                        {
                            !err && !loading && rows.length === 0 && (
                                <Table.Tr>
                                    <Table.Td colSpan={3}>
                                        No Team Records
                                    </Table.Td>
                                </Table.Tr>
                            )
                        }
                        {
                            rows.map((row) => (
                                <Table.Tr key={row.id}>
                                    <Table.Td>{row.id}</Table.Td>
                                    <Table.Td>{row.recordDate}</Table.Td>
                                    <Table.Td>{row.venueId}</Table.Td>
                                    <Table.Td>{row.teamId}</Table.Td>
                                    <Table.Td>{row.placed}</Table.Td>
                                    <Table.Td>{row.points}</Table.Td>
                                    <Table.Td>{row.teamName}</Table.Td>
                                    {/* Edit and Delete buttons*/}
                                    <Table.Td>
                                        <div style={{ display: "flex", justifyContent: "space-around" }}>
                                            <Button
                                                color="yellow"
                                            // onClick={() =>}
                                            >
                                                Edit
                                            </Button>
                                            <Button
                                                color="red"
                                            // onClick={() =>}
                                            >
                                                Edit
                                            </Button>
                                        </div>

                                    </Table.Td>
                                </Table.Tr>
                            ))
                        }
                    </Table.Tbody>
                </Table>
            </div>
            {/* Pagination controls */}
            <Group justify="space-between" mt="sm" wrap="wrap">
                <div style={{fontSize: 12, opacity: 0.8}}>
                    Page {page}{total ? ` / ${pages}` : ''}
                    {total ? ` • ${total} total` : ''}
                </div>
                <Group>
                    <Button variant="default" onClick={() => setPage((p) => Math.max(1,p-1))} disabled={page ===1}>
                        Prev
                    </Button>
                    <Button
                        onClick={() => setPage((p) => p + 1)}
                        disabled={total ? page >= pages: !hasNextHeuristic}
                    >
                        Next
                    </Button>
                </Group>
                {/* Create Dialog Component*/}
                {/* Edit Dialog Component*/}
            </Group>                
        </div>
    );
}