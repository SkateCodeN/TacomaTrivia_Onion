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
import { usersApi } from '../api/usersApi';


export default function CreateVenueDialog({opened, onClose, onCreated}){
    //Keep state of all the form inputs
    const [name, setName] = useState('');
    const[role, setRole] = useState('');
    const[phone,setPhone] = useState('');
    const[email, setEmail] = useState('');

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

    // Validate our inputs
    const validate =() => {
        if(!name.trim()) return 'Name is required';
        if(!email.trim()) return 'Email is required';
        if(!role) return 'Role is required';
        if(phone && phone.replace(/\D/g,'').length !== 10) return 'Phone must be 10 digits or leave it blank';
        return null;
    };

    const reset = () =>{
        setName('');
        setEmail('');
        setPhone('');
        setRole('');
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
                phone: phone || null,
                role: role.trim() || null,
                email: email.trim() || null
            }
            await usersApi.create(dto);
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
                        label="Email"
                        placeholder='ex: user@gmail.com'
                        value={email}
                        onChange={(event) => setEmail(event.target.value)}
                        required
                    />

                    <Group grow wrap="wrap">
                        <Select 
                            label="Role"
                            data={
                                [
                                    {value: 'Admin', label: 'Admin'},
                                    {value: 'Mod', label:"Moderator"},
                                    {value: 'User', label:"User"}
                                ]
                            }
                            value={role}
                            onChange={setRole}
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
                            Create User
                        </Button>
                    </Group>

                </Stack>

            </Card>

        </Modal>
    );
}

