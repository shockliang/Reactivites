import React from "react";
import {Header, Menu} from "semantic-ui-react";
import Calendar from 'react-calendar';

export default function ActivityFilter() {
    return (
        <>
            <Menu vertical size={'large'} style={{width: '100%', marginTop: 25}}>
                <Header icon={'filter'} attached={"top"} color={'teal'} content={'Filters'}/>
                <Menu.Item content={'All activities'}/>
                <Menu.Item content={"I'm going"}/>
                <Menu.Item content={"I'm hosting"}/>
            </Menu>
            <Header />
            <Calendar />
        </>
    )
}
