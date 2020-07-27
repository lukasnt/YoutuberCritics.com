import React, { useState } from "react";
import { Button, useMediaQuery, Menu, MenuItem, IconButton } from '@material-ui/core';
import AppBar from '@material-ui/core/AppBar';
import Toolbar from '@material-ui/core/Toolbar';
import SearchBar from "../searchbar";
import Typography from '@material-ui/core/Typography';
import Grid from '@material-ui/core/Grid';
import "./style.css";
import { Link } from "react-router-dom";
import { frontendDomain } from "../../App";
import MenuIcon from '@material-ui/icons/Menu';

export default function Navbar()
{
    const pad = useMediaQuery("(max-width: 1300px)")

    function clickHome(event){
        window.location.replace(frontendDomain);
    }

    const [anchorEl, setAnchorEl] = useState(null);
    const menuOpen = Boolean(anchorEl);
    const handleClick = (event) => {
        setAnchorEl(event.currentTarget);
      };
    
      const handleClose = () => {
        setAnchorEl(null);
      };

    return (
        <AppBar color="secondary" position="static" style={{zIndex: 2, position: "relative"}}>
            <Toolbar className="container">
                <Grid container alignItems="center">
                    <Grid item xs={2}> <Link color="primary" style={{color: "white"}} underline="none"> <Typography onClick={clickHome} className="title" variant="h5"> {pad ? "YTC" : "YoutuberCritics"} </Typography> </Link> </Grid>
                    <Grid item xs={6}> <SearchBar className="search" /> </Grid>
                    <Grid item xs={4}>
                        {pad ?
                        <div style={{display: "flex", justifyContent: "flex-end"}}>
                            <IconButton onClick={handleClick} onClose={handleClose} color="primary" >
                                <MenuIcon style={{fontSize: "36px"}}/>
                            </IconButton>
                            <Menu open={menuOpen} anchorEl={anchorEl} onClose={handleClose}>
                                <MenuItem onClick={handleClose}> <Typography variant="h6"> Channels </Typography> </MenuItem>
                                <MenuItem onClick={handleClose}> <Typography variant="h6"> Reviews </Typography> </MenuItem>
                                <MenuItem onClick={handleClose}> <Button variant="contained" color="secondary"> Log in </Button> </MenuItem>
                                <MenuItem onClick={handleClose}> <Button variant="contained" color="secondary"> Sign up </Button> </MenuItem>
                            </Menu>
                        </div>
                        : <Grid container justify="flex-end" spacing={3}>
                                <Grid item> <Typography variant="h6"> Channels </Typography>  </Grid>
                                <Grid item> <Typography variant="h6"> Reviews </Typography>  </Grid>
                                <Grid item> <Button variant="contained" color="secondary"> Log in </Button> </Grid>
                                <Grid item> <Button variant="contained" color="secondary"> Sign up </Button> </Grid>
                        </Grid>
                        } 
                    </Grid>
                </Grid>
            </Toolbar>
        </AppBar>
    )
}