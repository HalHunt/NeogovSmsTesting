import React from 'react';
import { InputGroup, InputGroupAddon, Button, Input } from 'reactstrap';

const SearchForm = props => {

    const [filter, setFilter] = React.useState("");

    return (

        <InputGroup className="mb-2">
            <Input type="search" className="mr-2" placeholder="Enter search criteria..." value={filter} onChange={event => setFilter(event.target.value)} />
            <InputGroupAddon addonType="append">
                <Button onClick={event => props.performSearch(filter)}>Search</Button>
            </InputGroupAddon>
        </InputGroup>
    )
}

export default SearchForm