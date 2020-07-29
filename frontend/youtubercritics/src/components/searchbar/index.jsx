import React from "react";
import SearchIcon from '@material-ui/icons/Search';
import { fade, makeStyles } from '@material-ui/core/styles';
import InputBase from '@material-ui/core/InputBase'
import { redirect } from "../../App";

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
  function handleKeyPress(event) {
    var searchInput = document.getElementById("searchInput");
    if (event.key === "Enter") {
        console.log("Keyword: " + searchInput.value);
        redirect("/search?keyword=" + searchInput.value);
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
    </div>
    );
}




