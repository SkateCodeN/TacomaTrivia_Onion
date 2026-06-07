import {
    Container,
    Stack,
    Button,
    Title
} from '@mantine/core';
import axios from 'axios';
import LoginForm from "./LoginForm";
export default function LoginPage() {
       
  
    const handleClick = () => {
        window.location.href ='/api/auth0/login';
    }
    return (
        <Container p="lg" size={500}>
            <LoginForm />

            <Stack p="lg">
                <Title order={3}>Auth0 Test</Title>
                <Button onClick={handleClick}>Login</Button>
            </Stack>
        </Container>


    );
}