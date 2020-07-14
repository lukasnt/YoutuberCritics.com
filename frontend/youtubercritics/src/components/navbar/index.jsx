import React, { useState } from "react";
import { Button } from '@material-ui/core';
import AppBar from '@material-ui/core/AppBar';
import Toolbar from '@material-ui/core/Toolbar';
import SearchBar from "../searchbar";
import Typography from '@material-ui/core/Typography';
import Grid from '@material-ui/core/Grid';
import "./style.css";
import { Redirect, Link } from "react-router-dom";

export default function Navbar()
{
    function clickHome(event){
        window.location.replace("http://localhost:3000/");
    }

    return (
        <AppBar color="secondary" position="static" style={{zIndex: 2, position: "relative"}}>
            <Toolbar className="container">
                <Grid container alignItems="center">
                    <Grid item xs={2}> <Link color="primary" style={{color: "white"}} underline="none"> <Typography onClick={clickHome} className="title" variant="h5"> YoutuberCritics </Typography> </Link> </Grid>
                    <Grid item xs={6}> <SearchBar className="search" /> </Grid>
                    <Grid item xs={4}>
                        <Grid container justify="flex-end" spacing={3}>
                                <Grid item> <Typography variant="h6"> Channels </Typography>  </Grid>
                                <Grid item> <Typography variant="h6"> Reviews </Typography>  </Grid>
                                <Grid item> <Button variant="contained" color="secondary"> Log in </Button> </Grid>
                                <Grid item> <Button variant="contained" color="secondary"> Sign up </Button> </Grid>
                        </Grid> 
                    </Grid>
                </Grid>
            </Toolbar>
        </AppBar>
    )
}