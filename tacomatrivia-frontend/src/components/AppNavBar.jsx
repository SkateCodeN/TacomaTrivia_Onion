// src/components/AppNavbar.jsx

import { useState } from 'react';
import {
    Container,
    Group,
    Burger,
    Drawer,
    Stack,
    Button,
    Text,
    Box,
    Collapse
} from '@mantine/core';
import { useDisclosure } from '@mantine/hooks';
import styles from '../styles/AppNavbar.module.css';

export default function AppNavbar() {
    const [opened, { toggle, close }] = useDisclosure(false);

    const navLinks = (
        <>
            <Text className={styles.link}>Events</Text>
            <Text className={styles.link}>Venues</Text>
            <Text className={styles.link}>Categories</Text>
            <Text className={styles.link}>About</Text>
        </>
    );

    return (
        <Box className={styles.wrapper} style={{display: opened ? 'none' : 'block'}}>

            {/* className={styles.container} */}
            <Container size="lg" >
                {/* Desktop Links */}
                <Group justify='space-between' h={70}>
                    <Text className={styles.logo}>
                        Tacoma Trivia
                    </Text>

                    <Group gap={28} visibleFrom='sm'>
                        {navLinks}

                        <Button radius='xl' className={styles.cta}>
                            Submit Event
                        </Button>
                    </Group>

                    {/* Mobile Burger */}
                    <Burger
                        opened={opened}
                        onClick={toggle}
                        hiddenFrom="sm"
                        aria-label="Open navigation menu"
                    />
                </Group>




            </Container>

            {/* Mobile Drawer */}
            <Drawer
                opened={opened}
                onClose={close}
                size="75%"
                padding="md"
                title="Tacoma Trivia"
            >
                <Stack>

                    <Text className={styles.mobileLink}>Events</Text>
                    <Text className={styles.mobileLink}>Venues</Text>
                    <Text className={styles.mobileLink}>Categories</Text>
                    <Text className={styles.mobileLink}>About</Text>
                    <Collapse in={opened}>
                        <Button
                            radius="xl"
                            fullWidth
                            className={styles.cta}
                            mt="md"
                        >
                            Submit Event
                        </Button>
                    </Collapse>


                </Stack>
            </Drawer>

        </Box>
    );
}