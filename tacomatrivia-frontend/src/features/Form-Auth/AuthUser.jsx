import {
    Container,
    Stack,
    Button,
    Group,
    Text,
    Title
}
    from '@mantine/core'
import axios from 'axios';
import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import Auth0User from './Auth0User';

export default function AuthUser() {
    const [test1, setTest1] = useState('');
    const [test2, setTest2] = useState('');
    const [test3, setTest3] = useState('');
    const [userData, setUserData] = useState(localStorage.getItem('authuserrole'));

    const navigate = useNavigate();
    const handleClick = () => {
        const token = localStorage.getItem('token');
        // Send http request to test product contronller. 
        // we expect ok status

        axios.get('/api/products', {
            headers: {
                'Authorization': `Bearer ${token}`
            }
        })
            .then(response => setTest1(response.data))
            .catch(error => console.error(error));


    }

    const Test2 = () => {
        const token = localStorage.getItem('token');
        // Send http request to test product contronller. 
        // we expect ok status

        axios.post('/api/products', {}, {
            headers: {
                'Authorization': `Bearer ${token}`
            }
        })
            .then(response => setTest2(response.data))
            .catch(error => {
                if (error.response && error.response.status === 403) {
                    console.log("Caught the 401!");
                    setTest2("Unauthorized");
                } else {
                    console.error("Different error:", error.message);
                }
            });


    }

    const Test3 = () => {
        const token = localStorage.getItem('token');
        // Send http request to test product contronller. 
        // we expect ok status

        axios.delete('/api/products/123', {
            headers: {
                'Authorization': `Bearer ${token}`
            }
        })
            .then(response => setTest3(response.data))
            .catch(error => {
                if (error.response && error.response.status === 403) {
                    console.log("Caught the 401!");
                    setTest3("Unauthorized");
                } else {
                    console.error("Different error:", error.message);
                }
            });


    }

    const handleAuthLogout = () => {
        window.location.href = '/api/auth0/logout';
    }
    const handleLogout = () => {

        //Remove the token from storages
        localStorage.removeItem('token');

        //navigate back to our userlogin page
        navigate('/login', { replace: true })

    }
    console.log("data form local storage: ", localStorage.getItem('authuserrole'))
    return (
        <Container p="lg" size={500}>
            <Stack>
                <Title order={3}>Authenticated User</Title>
                {userData && <Title order={6}>Role: {userData}</Title>}
            </Stack>
            <Group p="lg">
                <Button variant='light' onClick={handleClick}>Get Product</Button>

                <Text> Result:</Text>
                {
                    test1 &&
                    <Text>{test1}</Text>
                }
            </Group>
            <Group p="lg">
                <Button variant='light' onClick={Test2}>Create Product</Button>

                <Text> Result:</Text>
                {
                    test2 &&
                    <Text>{test2}</Text>
                }
            </Group>
            <Group p="lg">
                <Button variant='light' onClick={Test3}>Delete Product</Button>

                <Text> Result:</Text>
                {
                    test3 &&
                    <Text>{test3}</Text>
                }
            </Group>
            <Group justify='center'>
                <Button onClick={handleLogout}>Logout</Button>
                <Button onClick={handleAuthLogout}>Auth0 Logout</Button>
            </Group>

            <Group p="lg">
                <Auth0User />
            </Group>
        </Container>
    )
}