import React from 'react';
import Button from 'react-bootstrap/Button';
import Popover from 'react-bootstrap/Popover';
import OverlayTrigger from 'react-bootstrap/OverlayTrigger';

const styles = {
    relation: {
      color: 'rgba(0, 123, 255, 1.0)',
      //blue
    },
    type: {
      color: 'rgba(255, 193, 7, 1.0)',
      //yellow
    },
    property: {
      color: 'rgba(30, 138, 55, 1.0)',
      //green
    },
    unit: {
      color: 'rgba(25, 197, 224, 1.0)',
      //cyan
    },
    unknown: {
      color: 'rgba(220, 53, 69, 1.0)',
      //red
    },
};

//Each of these functions determines the contents of each popover.
const popoverExample = (
    <Popover id="popover-basic">
      <Popover.Content>
      All <span style={styles.type}>windows</span> should have a <span style={styles.property}>width</span> of <span style={styles.unit}>no less than 15 inch</span> and a <span style={styles.property}>height</span> of <span style={styles.unit}>no more than 2 feet</span> and is <span style={styles.relation}>above</span> a <span style={styles.type}>sink</span>.
      </Popover.Content>
    </Popover>
);

const popoverType = (
    <Popover id="popover-basic">
      <Popover.Content>
      The type of the object (Chair, Refrigerator, etc.).
      </Popover.Content>
    </Popover>
);

const popoverProperty = (
    <Popover id="popover-basic">
      <Popover.Content>
      Properties the object possesses (width, height, etc.)
      </Popover.Content>
    </Popover>
);

const popoverRelation = (
    <Popover id="popover-basic">
      <Popover.Content>
      The relationship between objects (directly against, above, below, between).
      </Popover.Content>
    </Popover>
);

const popoverUnit = (
    <Popover id="popover-basic">
      <Popover.Content>
      Units of measurement used (in, ft, m).
      </Popover.Content>
    </Popover>
);

const popoverUnknown = (
    <Popover id="popover-basic">
      <Popover.Content>
      Unrecognized component.
      </Popover.Content>
    </Popover>
);

export const popoverSuggestion = (
    <Popover id="popover-basic">
      <Popover.Content>
      Here some suggested words ;)
      </Popover.Content>
    </Popover>
);

//Buttons to display the popovers that were defined above. Buttons are called in App.js to be displayed.
export const ExampleButton = () => (
    <OverlayTrigger trigger="click" placement="top" overlay={popoverExample}>
      <Button variant="primary">Example</Button>
    </OverlayTrigger>
);

export const TypeButton = () => (
    <OverlayTrigger trigger={["hover", "focus"]} placement="top" overlay={popoverType}>
      <Button variant="outline-warning">Type</Button>
    </OverlayTrigger>
);

export const PropertyButton = () => (
    <OverlayTrigger trigger={["hover", "focus"]} placement="top" overlay={popoverProperty}>
      <Button variant="outline-success">Property</Button>
    </OverlayTrigger>
);

export const RelationButton = () => (
    <OverlayTrigger trigger={["hover", "focus"]} placement="top" overlay={popoverRelation}>
      <Button variant="outline-primary">Relation</Button>
    </OverlayTrigger>
);

export const UnitButton = () => (
    <OverlayTrigger trigger={["hover", "focus"]} placement="top" overlay={popoverUnit}>
      <Button variant="outline-info">Unit</Button>
    </OverlayTrigger>
);

export const UnknownButton = () => (
    <OverlayTrigger trigger={["hover", "focus"]} placement="top" overlay={popoverUnknown}>
      <Button variant="outline-danger">Unknown</Button>
    </OverlayTrigger>
);




