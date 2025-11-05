import { useMemo, useState,useEffect } from 'react';
import {
    Modal,
    Card,
    Stack,
    Group,
    TextInput,
    Select,
    NumberInput,
    Button,
    Text,
    Divider
} from '@mantine/core';
import { venuesApi } from '../api/venuesApi';
import { normalizeTime } from '../helper functions/normalizeTime.jsx';
// Helper function to map our days to the according number, 
// the Select will use this data.
const DAY_OPTIONS = [
    { value: '1', label: 'Monday' },
    { value: '2', label: 'Tuesday' },
    { value: '3', label: 'Wednesday' },
    { value: '4', label: 'Thursday' },
    { value: '5', label: 'Friday' },
    { value: '6', label: 'Saturday' },
    { value: '0', label: 'Sunday' },
];


export default function CreateVenueDialog({ opened, onClose, rowData, onCreated }) {
    //Keep state of all the form inputs
    const [name, setName] = useState('');
    const [allowsPets, setAllowsPets] = useState('false');
    const [rounds, setRounds] = useState(0);
    const [phone, setPhone] = useState('');
    const [website, setWebsite] = useState('');
    const [address, setAddress] = useState('');
    const [triviaDay, setTriviaDay] = useState(0);
    const [triviaStart, setTriviaStart] = useState('');
    const [allowsKids, setAllowsKids] = useState('false');
    const [id, setId] = useState('');
    //For error and to keeptrack of the submit state
    const [submitting, setSubmitting] = useState(false);
    const [err, setErr] = useState('');

    //Helper functions]
    // hydrate when rowData changes (and when modal opens)
    useEffect(() => {
        if (!opened || !rowData) return;
        setName(rowData.name ?? '');
        setAllowsPets(rowData.allowsPets ?? 'false');
        setRounds(rowData.rounds ?? 0);
        setPhone(rowData.phone ?? '');
        setWebsite(rowData.website ?? '');
        setAddress(rowData.address ?? '');
        setTriviaDay(String(rowData.triviaDay ?? ""));
        setTriviaStart(rowData.triviaStart ?? '');
        setAllowsKids(rowData.allowsKids ?? 'false');
        setId(rowData.id);

        setErr('');
        //console.log('row data', rowData);
    }, [opened, rowData]);

    //Formats phone to ex (123) 456-7890
    const prettyPhone = useMemo(() => {
        const d = phone.replace(/\D/g, '').slice(0, 10);
        //eg (123) 456-7890 formatting (optional)
        if (d.length <= 3) return d;
        if (d.length <= 6) return `(${d.slice(0, 3)}) ${d.slice(3)}`;
        return `(${d.slice(0, 3)}) ${d.slice(3, 6)}-${d.slice(6)}`;

    }, [phone]);

    //Formats phone to only be 10 digits long
    function setPhoneDigits(v) {
        const d = v.replace(/\D/g, '').slice(0, 10);
        setPhone(d);
    }

    // Validate name, address, triviaday, and trivia start
    const validate = () => {
        if (!name.trim()) return 'Name is required';
        if (!address.trim()) return 'Address is required';
        if (!triviaDay) return 'Trivia Day is required';
        const norm = normalizeTime(triviaStart);
        if (!norm) return 'Trivia start time is invalid use 21:00 or 9:00 pm';
        const r = Number(rounds);
        if (!Number.isFinite(r) || r < 1 || r > 20) return 'Rounds must be between 1 and 20';
        if (phone && phone.replace(/\D/g, '').length !== 10) return 'Phone must be 10 digits or leave it blank';
        return null;
    };

    const reset = () => {
        setName('');
        setAddress('');
        setAllowsPets(false);
        setRounds(0);
        setPhone('');
        setTriviaDay('');
        setTriviaStart('');
        setWebsite('');
        setAllowsKids(false);
        setErr('');
    }

    const handleSubmit = async () => {
        const v = validate();

        if (v) { setErr(v); return; }
        setSubmitting(true);
        setErr('');

        try {
            const dto = {
                name: name.trim(),
                allowsPets: allowsPets === "True",
                rounds: Number(rounds),
                phone: phone || null,
                address: address.trim(),
                triviaDay: Number(triviaDay),
                triviaStart: normalizeTime(triviaStart),
                website: website.trim() || null,
                allowsKids: allowsKids === 'True'
            }
            await venuesApi.edit(id,dto);
            onCreated?.(); //parent to fetch list
            reset();
            onClose?.();

        }
        catch (error) {
            setErr(error?.message || 'Failed to create venue');
        }
        finally {
            setSubmitting(false);
        }
    };

    
    return (
        <Modal
            opened={opened}
            onClose={onClose}
            title="Edit Venue"
            centered size='lg'
            lockScroll
        >
            <Card
                withBorder
                radius='lg'
                padding='md'
            >
                <Stack gap='sm'>
                    <Group grow wrap='wrap'>
                        <TextInput
                            label="Name"
                            placeholder="ex.Doyle's Public House"
                            value={name}
                            onChange={(event) => setName(event.target.value)}
                            required
                        />

                        <TextInput
                            label="Phone"
                            placeholder='(123) 456-7890'
                            value={prettyPhone}
                            onChange={(event) => setPhoneDigits(event.target.value)}
                            maxLength={14}
                        />

                    </Group>

                    <TextInput
                        label="Address"
                        placeholder='123 Main St, Tacoma, WA'
                        value={address}
                        onChange={(event) => setAddress(event.target.value)}
                        required
                    />

                    <Group grow wrap="wrap">
                        <Select
                            label="Allows Pets"
                            data={
                                [
                                    { value: 'true', label: 'True' },
                                    { value: 'false', label: "False" }
                                ]
                            }
                            value={allowsPets}
                            onChange={setAllowsPets}
                            allowDeselect={false}
                        />
                        <NumberInput
                            label="Rounds"
                            min={1}
                            max={20}
                            value={rowData.rounds || rounds.toString()}
                            onChange={(v) => setRounds(Number(v) || 1)}
                        />
                    </Group>

                    <Group grow wrap="wrap">
                        <Select
                            label="Trivia Day"
                            placeholder='Select Day'
                            data={DAY_OPTIONS}
                            value={triviaDay}
                            onChange={setTriviaDay}
                            allowDeselect={false}
                        />
                        {/* Native time input to avoid extra dependencies.
                            Accepts 24h (ex 19:00)
                        */}
                        <TextInput
                            label='Trivia Start'
                            placeholder='e.g 21:00 or 9:00 pm'
                            value={triviaStart}
                            onChange={(event) => setTriviaStart(event.target.value)}
                        // description ="Use 24hr ex 21:00 or am/pm (9:00 pm)"
                        />
                    </Group>

                    <Group grow wrap="wrap">
                        <TextInput
                            label="Website"
                            placeholder='doylespub.com'
                            value={website}
                            onChange={(event) => setWebsite(event.target.value)}
                        />

                        <Select
                            label="Allows Kids"
                            value={allowsKids}
                            data={
                                [
                                    { value: 'true', label: "True" },
                                    { value: 'false', label: "False" }
                                ]
                            }
                            onChange={setAllowsKids}
                            allowDeselect={false}
                        />
                    </Group>

                    {err && <Text c='red'>{err}</Text>}

                    <Divider />

                    <Group justify='flex-end'>
                        <Button
                            variant='default'
                            onClick={onClose}
                            disabled={submitting}
                        >
                            Cancel
                        </Button>
                        <Button
                            onClick={handleSubmit}
                            loading={submitting}
                        >
                            Edit Venue
                        </Button>
                    </Group>

                </Stack>

            </Card>

        </Modal>
    );
}

