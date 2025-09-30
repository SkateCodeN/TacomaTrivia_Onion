import { useMemo, useState } from 'react';
import {
    Modal,
    Card,
    Stack,
    Group,
    TextInput,
    Textarea,
    Select,
    NumberInput,
    Button,
    Text,
    Divider
} from '@mantine/core';

export default function CreateVenueDialog({opened, onClose}){
    
    //For error and to keeptrack of the submit state
    const[submitting, setSubmitting] = useState(false);
  
    const reset = () =>{
        
    }

    const handleSubmit = async() =>{
        
    };

    return(
        <Modal
            opened={opened}
            onClose={onClose}
            title="Delete Venue"
            centered size = 'lg'
            lockScroll
        >
            <Card
                withBorder 
                radius='lg'
                padding='md'
            >
                <Stack gap='sm'>
                    <Group grow wrap='nowrap'>
                       <Text>
                            Please submit a request ticket to delete this venue and 
                            if you could give me a reason to keep in the record.
                       </Text>
                        
                    </Group>
                    <Group grow>
                        <Textarea
                            label="Reason"
                            placeholder='Please delete because...'
                            autosize
                            minRows={4}
                        ></Textarea>
                    </Group>

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
                            Request Delete
                        </Button>
                    </Group>

                </Stack>

            </Card>

        </Modal>
    );
}

