// src/components/AppNavbar.jsx

import { useState } from 'react';
import {Link} from 'react-router-dom'
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
import NavAuthComp from './NavAuthComp';
export default function AppNavbar() {
    const [opened, { toggle, close }] = useDisclosure(false);

    const navLinks = (
        <>
            <Link to='venues' className={styles.link}>Venues</Link>
            <Text className={styles.link}>Teams</Text>
            <Text className={styles.link}>Docs</Text>
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

                        <NavAuthComp />
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

                    <Link to='venues' className={styles.link}>Venues</Link>
                    <Text className={styles.mobileLink}>Teams</Text>
                    <Text className={styles.mobileLink}>Docs</Text>
                    <Text className={styles.mobileLink}>About</Text>
                    <Collapse in={opened}>
                        <NavAuthComp />
                    </Collapse>


                </Stack>
            </Drawer>

        </Box>
    );
}