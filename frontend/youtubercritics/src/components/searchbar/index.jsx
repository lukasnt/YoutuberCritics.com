import React, { useState } from "react";
import { TextField } from "@material-ui/core";
import SearchIcon from '@material-ui/icons/Search';
import { fade, makeStyles } from '@material-ui/core/styles';
import Typography from '@material-ui/core/Typography'
import InputBase from '@material-ui/core/InputBase'
import { Route, Redirect } from "react-router-dom";

const useStyles = makeStyles((theme) => ({
    root: {
        flexGrow: 1,
    },
    menuButton: {
        marginRight: theme.spacing(2),
    },
    title: {
        flexGrow: 1,
        display: 'none',
        /*
        [theme.breakpoints.up('sm')]: {
        display: 'block',
        },
        */
    },
    search: {
        position: 'relative',
        borderRadius: theme.shape.borderRadius,
        backgroundColor: fade(theme.palette.common.black, 0.15),
        '&:hover': {
        backgroundColor: fade(theme.palette.common.black, 0.25),
        },
        marginLeft: 0,
        width: '100%',
        /*
        [theme.breakpoints.up('sm')]: {
        marginLeft: theme.spacing(1),
        width: 'auto',
        },
        */
    },
    searchIcon: {
        padding: theme.spacing(0, 2),
        height: '100%',
        position: 'absolute',
        pointerEvents: 'none',
        display: 'flex',
        alignItems: 'center',
        justifyContent: 'center',
    },
    inputRoot: {
        color: 'inherit',
    },
    inputInput: {
        padding: theme.spacing(1, 1, 1, 0),
        // vertical padding + font size from searchIcon
        paddingLeft: `calc(1em + ${theme.spacing(4)}px)`,
        //transition: theme.transitions.create('width'),
        width: '100%',
        /*
        [theme.breakpoints.up('sm')]: {
        width: '12ch',
        '&:focus': {
            width: '30ch',
        },
        },
        */
    },
}));

export default function SearchBar() {
  const classes = useStyles();
  const[redirect, setRedirect] = useState(null);
  const[keyword, setKeyword] = useState("");
  function handleKeyPress(event) {
    var searchInput = document.getElementById("searchInput");
    if (event.key === "Enter") {
        console.log("Keyword: " + searchInput.value);
        setKeyword(document.getElementById("searchInput").value);
        window.location.replace("http://localhost:3000/search?keyword=" + searchInput.value);
    }
   }

  return (
    <div className={classes.search}>
        <div className={classes.searchIcon}>
            <SearchIcon />
        </div>
        <InputBase
            id="searchInput"
            placeholder="Search…"
            classes={{
                root: classes.inputRoot,
                input: classes.inputInput,
            }}
            inputProps={{ 'aria-label': 'search' }}
            onKeyPress={handleKeyPress}
        />
        {redirect}
    </div>
    );
}




