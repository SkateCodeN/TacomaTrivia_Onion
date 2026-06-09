import { useMemo, useState } from 'react';
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

// Helper func accepts 21:00, 9:00 pm, 9pm, 9:00
function normalizeTime(input) {
    if (!input) {
        return '';
    }
    const s = input.trim().toLowerCase().replace(/\s+/g, '');

    // HH:mm
    const hhmm = s.match(/^(\d{1,2}):(\d{2})(?:[:.](\d{2}))?$/);

    if (hhmm) {
        let [, h, m, sec] = hhmm;
        h = String(Math.min(23, Math.max(0, parseInt(h, 10)))).padStart(2, '0');
        m = String(Math.min(59, Math.max(0, parseInt(m, 10)))).padStart(2, '0');

        sec = sec ? String(Math.min(59, Math.max(0, parseInt(sec, 10)))).padStart(2, '0') : '00';
        return `${h}:${m}:${sec}`;
    }

    //h[:mm]am/pm
    const ampm = s.match(/^(\d{1,2})(?::(\d{2}))?(am|pm)$/);
    if (ampm) {
        let [, h, m, ap] = ampm;
        h = parseInt(h, 10);
        m = m ? parseInt(m, 10) : 0;
        if (ap === 'pm' && h < 12) h += 12;
        if (ap === 'am' && h === 12) h = 0;

        const hh = String(h).padStart(2, '0');
        const mm = String(m).padStart(2, '0');

        return `${hh}:${mm}:00`;
    }

    return '';

}

export default function CreateVenueDialog({opened, onClose}){
    //Keep state of all the form inputs
    const [name, setName] = useState('');
    const[allowsPets, setAllowsPets] = useState('false');
    const[rounds, setRounds] = useState(0);
    const[phone,setPhone] = useState('');
    const[website, setWebsite] = useState('');
    const[address,setAddress] = useState('');
    const[triviaDay,setTriviaDay] = useState(0);
    const[triviaStart,setTriviaStart] = useState('');
    const[allowsKids, setAllowsKids] = useState('false');

    //For error and to keeptrack of the submit state
    const[submitting, setSubmitting] = useState(false);
    const[err,setErr] = useState('');

    //Helper functions
    //validate only 10 digits for phone
    const prettyPhone = useMemo(() => {
        const d = phone.replace(/\D/g, '').slice(0,10);
        //eg (123) 456-7890 formatting (optional)
        if(d.length <= 3) return d;
        if(d.length <= 6) return `(${d.slice(0,3)}) ${d.slice(3)}`;
        return `(${d.slice(0,3)}) ${d.slice(3,6)}-${d.slice(6)}`;
    
    },[phone]);

    function setPhoneDigits(v){
        const d = v.replace(/\D/g,'').slice(0,10);
        setPhone(d);
    }

    // Validate name, address, triviaday, and trivia start
    const validate =() => {
        if(!name.trim()) return 'Name is required';
        if(!address.trim()) return 'Address is required';
        if(!triviaDay) return 'Trivia Day is required';
        const norm = normalizeTime(triviaStart);
        if (!norm) return 'Trivia start time is invalid use 21:00 or 9:00 pm';
        const r = Number(rounds);
        if(!Number.isFinite(r) || r < 1 || r> 20) return 'Rounds must be between 1 and 20';
        if(phone && phone.replace(/\D/g,'').length !== 10) return 'Phone must be 10 digits or leave it blank';
        return null;
    };

    const reset = () =>{
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

    const handleSubmit = async() =>{
        const v = validate();

        if(v) { setErr(v); return;}
        setSubmitting(true);
        setErr('');

        try{
            const dto ={
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
            await venuesApi.create(dto);
            onCreated?.(); //parent to fetch list
            reset();
            onClose?.();

        }
        catch(error){
            setErr(error?.message || 'Failed to create venue');
        }
        finally{
            setSubmitting(false);
        }
    };

    return(
        <Modal
            opened={opened}
            onClose={onClose}
            title="Add new Venue"
            centered size = 'lg'
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
                                    {value: 'true', label: 'True'},
                                    {value: 'false', label:"False"}
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
                            value={rounds.toString()}
                            onChange={(v) => setRounds(Number(v) || 1)}
                        />  
                    </Group>

                    <Group grow wrap="wrap">
                        <Select 
                            label="Trivia Day"
                            placeholder='Select Day'
                            data ={DAY_OPTIONS}
                            value= {triviaDay}
                            onChange={setTriviaDay.toString()}
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
                            data = {
                                [
                                    {value:'true', label:"True"},
                                    {value:'false', label:"False"}
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
                            Create Venue
                        </Button>
                    </Group>

                </Stack>

            </Card>

        </Modal>
    );
}

