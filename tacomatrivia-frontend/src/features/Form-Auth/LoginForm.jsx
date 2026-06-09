import {
    Anchor,
    Button,
    Checkbox,
    Divider,
    Paper,
    PasswordInput,
    Text,
    TextInput,
    Title,
    Stack,
    Group
} from '@mantine/core';

import { useForm } from '@mantine/form';
import { upperFirst, useToggle } from '@mantine/hooks';
import axios from 'axios';

import { useNavigate } from 'react-router-dom';
export default function LoginForm() {
    const [type, toggle] = useToggle(['login', 'register']);
    const form = useForm({
        initialValues: {
            email: '',
            name: '',
            password: ''
        },

        validate: {
            email: (val) => (/^\S+@\S+$/.test(val) ? null : 'Invalid email'),
            password: (val) => (val.length <= 6 ? 'Password should include at least 6 characters' : null),
        },

    });

    const navigate = useNavigate();

    const handleSubmit = async (values) => {
        console.log(values);

        // Send details to backend
        try {
            // Example: Send to your .NET backend
            const response = await axios.post('/api/test-auth',values);
            
            if(response.data.token){
                // Save the token
                localStorage.setItem('token', response.data.token);
                // save the entire response
                localStorage.setItem('authuserrole', response.data.roleAssigned);
                //redirect to the auth page
                navigate('/auth/user', {replace: true})
            }
            // We should get a token
            console.log(response.data);
        }
        catch (error) {
            console.error("Auth Error:", error.response?.data || error.message);
        }
    }

     const handleAuth0Click = () => {
        window.location.href ='/api/auth0/login';
    }

    return (
        <Paper radius='md' p='lg' withBorder >
            <Group justify="space-between">
                <Text size='lg' fw={500} c="bright">
                Login To Tacoma Trivia
            </Text>

            <Button radius="xl" color="brand.8" onClick={handleAuth0Click} >
                Auth0
            </Button>
            </Group>
            
            <Divider my='lg' />

            <form onSubmit={form.onSubmit(handleSubmit)}>

                <Stack>
                    {/* Register Input */}
                    {type === 'register' && (
                        <TextInput
                            label="Name"
                            placeholder="Your Name"
                            value={form.values.name}
                            onChange={(event) => form.setFieldValue("name", event.currentTarget.value)}
                            radius='md'
                        />
                    )}
                    {/* Email Input */}
                    <TextInput
                        required
                        label="Email"
                        placeholder="hello@mantine.dev"
                        value={form.values.email}
                        onChange={(event) => form.setFieldValue('email', event.currentTarget.value)}
                        error={form.errors.email && 'Invalid email'}
                        radius="md"
                    />
                    {/* Password Input */}
                    <PasswordInput
                        required
                        label="Password"
                        placeholder="Your password"
                        value={form.values.password}
                        onChange={(event) => form.setFieldValue('password', event.currentTarget.value)}
                        error={form.errors.password && 'Password should include at least 6 characters'}
                        radius="md"
                    />
                </Stack>

                <Group justify="space-between" mt="xl">
                    <Anchor
                        component="button"
                        type="button"
                        c="bright"
                        opacity={0.85}
                        onClick={() => toggle()}
                        size="xs"
                    >
                        {type === 'register'
                            ? 'Already have an account? Login'
                            : "Don't have an account? Register"}
                    </Anchor>
                    <Button type="submit" radius="xl">
                        {upperFirst(type)}
                    </Button>
                </Group>
                
            </form>
        </Paper>
    );
}